/*****************************************************************************
// File Name : MenuBehavior.cs
// Author : Robert Lee
// Creation Date : March 2, 2020
//
// Brief Description :  Controls how the player can navigate the menu, as well
                        as what each option on the menu can do.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject levelSelect;
    public GameObject levelOne;
    public GameObject levelTwo;
    public GameObject levelThree;
    public GameObject LevelFour;
    public Button LevelTwoButton;
    public Button LevelThreeButton;
    public Button LevelFourButton;

    public static float ProgressCheck = 0.5f;

    private void Start()
    {
        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
        levelOne.SetActive(false);
        levelTwo.SetActive(false);
        levelThree.SetActive(false);
        LevelFour.SetActive(false);

        if (ProgressCheck < 1)
        {
            Object.Destroy(LevelTwoButton);
            Object.Destroy(LevelThreeButton);
            Object.Destroy(LevelFourButton);
        }
        else if (ProgressCheck < 2)
        {
            Object.Destroy(LevelThreeButton);
            Object.Destroy(LevelFourButton);
        }
        else if (ProgressCheck < 3)
        {
            Object.Destroy(LevelFourButton);
        }
    }

    /// <summary>
    /// Loads into the level of your choosing
    /// </summary>
    /// <param name="levelname">Name of level being loaded into</param>
    public void LoadLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }
}
