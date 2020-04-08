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
    private int playerLives = 3;
    public int maxPlayerLives = 3;

    //Is the player currently invincible (after taking damage)?
    private bool invincible;

    public bool playerAlive = true;

    [Tooltip("How long the player has invinciblilty after being hit (in seconds).")]
    public float invincibilityTime = 2f;


    [Header("General Player Attributes")]
    [Tooltip("UI text to display lives count.")]
    public GameObject livesText;

    private AudioController audioController;

    private void Awake()
    {
        //Singleton behaviour
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioController = GetComponentInChildren<AudioController>();
    }

    public void RemoveLivesFromPlayer(int livesToRemove)
    {
        if (invincible || !playerAlive)
            return;

        invincible = true;
        playerLives -= livesToRemove;
        audioController.PlayClip(AudioController.GameManagerSFX.playerHit);

        UpdateLives();
    }

    //This should run at the start of each level.
    public void PrepareLevel()
    {
        invincible = false;
        playerAlive = true;
        playerLives = maxPlayerLives;

        //If the livesText is null, find it and make sure it's active.
        if (livesText == null)
        {
            livesText = GameObject.Find("LivesText");
        }
        if (!livesText.activeSelf)
            livesText.SetActive(true);
        UpdateLives();

        transform.GetChild(1).GetComponent<AudioController>().PlayBackgroundMusic();
        //print("GameController: Prepared the level");
    }

    public void UpdateLives()
    {
        livesText.GetComponent<Text>().text = "Lives: " + playerLives;

        if(playerLives <= 0 && playerAlive)
        {
            print("*You are dead, de-de-dead.*");
            playerAlive = false;

            transform.GetChild(1).GetComponent<AudioController>().StopBackgroundMusic(); //Stop the background music.
            audioController.PlayClip(AudioController.GameManagerSFX.gameOver);
        }

        Invoke("RemoveInvincibility", invincibilityTime);
    }

    void RemoveInvincibility()
    {
        invincible = false;
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
    }
}