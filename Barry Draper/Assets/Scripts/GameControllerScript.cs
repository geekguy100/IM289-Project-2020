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

public class GameControllerScript : MonoBehaviour
{
    //Instance for singleton behaviour.
    public static GameControllerScript instance;

    [Header("Player Life Management")]
    [SerializeField] //Made this a [SerializeField] so we can edit it in the Unity editor but not in other classes.
    private int playerLives = 3;

    //Is the player currently invincible (after taking damage)?
    private bool invincible;

    public bool playerAlive = true;

    [Tooltip("How long the player has invinciblilty after being hit (in seconds).")]
    public float invincibilityTime = 2f;


    [Header("General Player Attributes")]
    [Tooltip("Player game object.")]
    public GameObject playerAvatar;
    [Tooltip("UI text to display lives count.")]
    public Text livesText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        invincible = false;

        UpdateLives();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets the "cheat" inputs to help with prototype debugging.
        Cheats();
    }

    public void RemoveLivesFromPlayer(int livesToRemove)
    {
        if (invincible)
            return;

        playerLives--;
        invincible = true;
        UpdateLives();
    }

    public void UpdateLives()
    {
        livesText.text = ("Lives: " + playerLives);

        if(playerLives <= 0 && playerAlive)
        {
            print("*You are dead, de-de-dead.*");
            playerAlive = false;
        }

        Invoke("RemoveInvincibility", invincibilityTime);
    }

    void RemoveInvincibility()
    {
        invincible = false;
    }

    /// <summary>
    /// For prototype presentation so if anything goes wrong we can reset.
    /// </summary>
    void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = new Vector3(-9.0f, 3.0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = new Vector3(21.0f, -5.0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject.Find("Test Box").transform.position = new Vector3(51.0f, -5.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Sample Level");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}