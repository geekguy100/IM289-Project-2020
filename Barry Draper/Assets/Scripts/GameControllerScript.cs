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
using System;

public class GameControllerScript : MonoBehaviour
{
    //Instance for singleton behaviour.
    public static GameControllerScript instance;

    [Header("Player Life Management")]
    private float playerLives = 5;
    public float maxPlayerLives = 5;
    public int hearts = 3;

    //Is the player currently invincible (after taking damage)?
    private bool invincible;

    [HideInInspector]
    public bool playerAlive = true;

    [Tooltip("How long the player has invinciblilty after being hit (in seconds).")]
    public float invincibilityTime = 2f;


    [Header("General Player Attributes")]
    [Tooltip("UI text to display lives count.")]
    public GameObject livesText;
    public GameObject Heart3;
    public GameObject Heart2;
    public GameObject Heart1;

    private AudioController audioController;

    [Header("Used to create life bar")]
    public Slider healthBar;

    private Vector2 checkpointPos;
    private bool hasCheckpoint = false;

    private void Awake()
    {
        //Singleton behaviour
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioController = GetComponentInChildren<AudioController>();

        //playerLives = maxPlayerLives;
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

        if(playerLives <= 0)
        {
            --hearts;
            switch (hearts)
            {
                case (2):
                    {
                        Heart3.SetActive(false);
                        break;
                    }
                case (1):
                    {
                        Heart2.SetActive(false);
                        break;
                    }
                case (0):
                    {
                        Heart1.SetActive(false);
                        break;
                    }

                default:
                break;
            }
            if(hearts >= 0)
            {
                healthBar.value = maxPlayerLives;
                AwardLives(3);
            }
        }
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
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();

        /*if (!livesText.activeSelf)
            livesText.SetActive(true);*/

        UpdateLives();
        hearts = 3;

        transform.GetChild(1).GetComponent<AudioController>().PlayBackgroundMusic();

        if (hasCheckpoint)
        {
            MovePlayerToCheckpoint();
        }
    }

    public void UpdateLives()
    {
        //livesText.GetComponent<Text>().text = "Lives: " + playerLives;
        healthBar.maxValue = maxPlayerLives;
        healthBar.value = playerLives;

        if (playerLives <= 0 && playerAlive && hearts <= 0)
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
        hasCheckpoint = false;
        //livesText.SetActive(false);
    }

    public void ResetCheckpointStatus()
    {
        hasCheckpoint = false;
    }

    public void RestartLevel()
    {
        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);

        /*if (checkpointPassed)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = checkpoint.transform.position;
        }*/
        PrepareLevel();

        /*if (checkpoint1)
        {
            player.transform.position = new Vector3(79.5f, 13.0f, 0f);
        }*/
    }

    public void OnGameComplete(GameObject levelCompleteCanvas)
    {
        FinishLevel();
        levelCompleteCanvas.SetActive(true);
    }

    public void UpdateCheckpointPos(Vector2 pos)
    {
        checkpointPos = pos;
        hasCheckpoint = true;
    }

    private void MovePlayerToCheckpoint()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = checkpointPos;
    }
}