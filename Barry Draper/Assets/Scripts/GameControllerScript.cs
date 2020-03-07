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
            //TODO: play a game over SFX.
            print("*You are dead, de-de-dead.*");
            playerAlive = false;
        }

        //TODO: play a player hit SFX.

        Invoke("RemoveInvincibility", invincibilityTime);
    }

    void RemoveInvincibility()
    {
        invincible = false;
    }
}