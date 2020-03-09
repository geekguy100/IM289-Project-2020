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

public class MenuBehavior : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject levelSelect;
    public GameObject levelOne;

    private void Start()
    {
        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
        levelOne.SetActive(false);
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
