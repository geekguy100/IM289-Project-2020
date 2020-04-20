/*****************************************************************************
// File Name : GameControllerScript.cs
// Author : Connor Dunn
// Creation Date : February 2, 2020
//
// Brief Description : Manages player lives, score, and other global values.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    //Instance for singleton behaviour.
    public static GameControllerScript instance;

    [Header("Player Life Management")]
    private float playerLives = 5;
    public float maxPlayerLives = 5;

    //Is the player currently invincible (after taking damage)?
    private bool invincible;

    [HideInInspector]
    public bool playerAlive = true;

    [Tooltip("How long the player has invinciblilty after being hit (in seconds).")]
    public float invincibilityTime = 2f;


    [Header("General Player Attributes")]
    [Tooltip("UI text to display lives count.")]
    public GameObject livesText;

    private AudioController audioController;

    [Header("For tracking if player hit checkpoint")]
    public GameObject player;
    bool checkpoint1 = false;
    string sceneName;

    [Header("Used to create life bar")]
    public Slider healthBar;

    private void Update()
    {
        //if (player.transform.position.y >= 13.0f &&
        //    player.transform.position.x >= 79 &&
        //    sceneName == "First Level")
        //{
        //    checkpoint1 = true;
        //}
    }

    private void Awake()
    {
        //Singleton behaviour
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioController = GetComponentInChildren<AudioController>();

        //Getting the current scene and it's name
        Scene currentScene = SceneManager.GetActiveScene();
       sceneName = currentScene.name;

        playerLives = maxPlayerLives;
        Debug.Log("health = " + playerLives);
        healthBar.maxValue = maxPlayerLives;
        Debug.Log("max value = " + maxPlayerLives);
        healthBar.value = playerLives;
        Debug.Log("Value = " + healthBar.value);
    }

    public void RemoveLivesFromPlayer(int livesToRemove)
    {
        if (invincible || !playerAlive)
            return;

        invincible = true;
        playerLives -= livesToRemove;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StartFlash();
        audioController.PlayClip(AudioController.GameManagerSFX.playerHit);
        healthBar.value -= 1;

        UpdateLives();
    }

    public bool AwardLives(int lives)
    {
        if (playerLives >= maxPlayerLives)
            return false;
        else
            playerLives += lives;

        if (playerLives > maxPlayerLives)
            playerLives = maxPlayerLives;

        //Play sound
        UpdateLives();

        return true;
    }

    //This should run at the start of each level.
    public void PrepareLevel()
    {
        Time.timeScale = 1;

        invincible = false;
        playerAlive = true;
        playerLives = maxPlayerLives;

        //If the livesText is null, find it and make sure it's active.
        livesText = GameObject.Find("LivesText");

        /*if (!livesText.activeSelf)
            livesText.SetActive(true);*/

        UpdateLives();

        transform.GetChild(1).GetComponent<AudioController>().PlayBackgroundMusic();
        //print("GameController: Prepared the level");
    }

    public void UpdateLives()
    {
        //livesText.GetComponent<Text>().text = "Lives: " + playerLives;

        if(playerLives <= 0 && playerAlive)
        {
            print("*You are dead, de-de-dead.*");
            playerAlive = false;

            transform.GetChild(1).GetComponent<AudioController>().StopBackgroundMusic(); //Stop the background music.
            audioController.PlayClip(AudioController.GameManagerSFX.gameOver);
            GameObject.Find("Menu Controller").GetComponent<PauseMenuBehavior>().ShowPauseAtGameEnd();
        }

        Invoke("RemoveInvincibility", invincibilityTime);
    }

    void RemoveInvincibility()
    {
        invincible = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StopFlash();
    }

    public void FinishLevel()
    {
        audioController.PlayClip(AudioController.GameManagerSFX.finishLevel);
        livesText.SetActive(false);
    }

    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
        PrepareLevel();
        if(checkpoint1)
        {
            player.transform.position = new Vector3(79.5f, 13.0f, 0f);
        }
    }

    public void OnGameComplete(GameObject levelCompleteCanvas)
    {
        FinishLevel();
        levelCompleteCanvas.SetActive(true);
    }
}