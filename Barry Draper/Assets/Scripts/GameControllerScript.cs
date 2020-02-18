/*****************************************************************************
// File Name : GameControllerScript.cs
// Author : Connor Dunn
// Creation Date : February 2, 2020
//
// Brief Description : [DESCRIPTION]
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static int playerLives;

    /// <summary>
    /// Does the player have invinciblility frames?
    /// </summary>
    public static bool invincible;

    [Tooltip("How long the player has invinciblilty frames after being hit.")]
    public float invincibilityTime = 2;

    [Tooltip("UI text to display lives count.")]
    public Text livesText;

    [Tooltip("Player game object.")]
    public GameObject playerAvatar;

    // Start is called before the first frame update
    void Start()
    {
        playerLives = 3;
        invincible = false;

        UpdateLives();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.livesChanged)
        {
            UpdateLives();
            PlayerController.livesChanged = false;
        }
    }

    public void UpdateLives()
    {
        livesText.text = ("Lives: " + playerLives);

        if(playerLives <= 0)
        {
            print("*You are dead, de-de-dead.*");
            PlayerController.isAlive = false;
        }

        Invoke("RemoveInvincibility", invincibilityTime);
    }

    void RemoveInvincibility()
    {
        invincible = false;
    }
}