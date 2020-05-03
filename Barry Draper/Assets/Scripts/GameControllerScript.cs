/*****************************************************************************
// File Name : GameControllerScript.cs
// Author : Connor Dunn
// Creation Date : February 2, 2020
//
// Brief Description : Manages player lives, score, and other global values.
*****************************************************************************/
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
    public GameObject Heart0;

    private AudioController audioController;

    [Header("Used to create life bar")]
    public Slider healthBar;

    private Vector2 checkpointPos;
    private bool hasCheckpoint = false;

    private bool freeCamMode = false;

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


    public void RemoveLivesFromPlayer(float livesToRemove)
    {
        if (invincible || !playerAlive)
            return;

        invincible = true;
        playerLives -= livesToRemove;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StartFlash();
        audioController.PlayClip(AudioController.GameManagerSFX.playerHit);
        //healthBar.value -= 1;
        healthBar.value -= livesToRemove;
        //print(playerLives);

        UpdateLives();

        if(playerLives <= 0)
        {
            --hearts;
            switch (hearts)
            {
                case (2):
                    {
                        Heart3.SetActive(false);
                        Heart2.SetActive(true);
                        break;
                    }
                case (1):
                    {
                        Heart2.SetActive(false);
                        Heart1.SetActive(true);
                        break;
                    }
                case (0):
                    {
                        Heart1.SetActive(false);
                        Heart0.SetActive(true);
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
        SetFreeCamMode(false);

        //If the livesText is null, find it and make sure it's active.
        livesText = GameObject.Find("LivesText");
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();

        /*if (!livesText.activeSelf)
            livesText.SetActive(true);*/

        UpdateLives();
        hearts = 3;
        Heart3 = GameObject.Find("Heart3");
        Heart2 = GameObject.Find("Heart2");
        Heart1 = GameObject.Find("Heart1");

        Heart2.SetActive(false);
        Heart1.SetActive(false);
        Heart0.SetActive(false);

        if (hasCheckpoint)
        {
            MovePlayerToCheckpoint();
        }

        transform.GetChild(1).GetComponent<AudioController>().PlayBackgroundMusic();
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
        ResetCheckpointStatus();
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

    //CameraBehaviour.cs toggles the freeCamMode boolean between true and false.
    //In this script, freeCamMode is used to determine if the player can move or not. In free cam mode, they should NOT be able to.
    public void SetFreeCamMode(bool freeCam)
    {
        freeCamMode = freeCam;
    }

    public bool GetFreeCamMode()
    {
        return freeCamMode;
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