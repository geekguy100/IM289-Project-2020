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

    private void Start()
    {
        titleScreen.SetActive(true);
        levelSelect.SetActive(false);
    }


    public void LoadLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }
}
