/*****************************************************************************
// File Name : MenuBehavior.cs
// Author : Robert Lee
// Creation Date : March 4, 2020
//
// Brief Description :  Controls how the player can pause and unpause the game
                        as well as control menu options within the pause menu.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuBehavior : MonoBehaviour
{

    public static bool isPaused;

    public GameObject pauseMenu;
    public GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            //If you are on options menu and press escape, quit to pause menu
            if (optionsMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
            }
            else
            {
                isPaused = !isPaused;

                if (isPaused)
                {
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                }
                else
                {
                    Time.timeScale = 1;
                    pauseMenu.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Resumes the game from the pause state
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Restarts the scene that is being played at that time
    /// </summary>
    public void RestartGame()
    {
        string levelName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelName);
    }

    /// <summary>
    /// Goes back to the main menu scene
    /// </summary>
    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }
}