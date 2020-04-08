/*****************************************************************************
// File Name : ProgressCheck.cs
// Author : Robert Lee
// Creation Date : April 7, 2020
//
// Brief Description :  Keeps track of data and saves data as the player 
                        progresses through the game, even when the player exits
                        the game.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressCheck : MonoBehaviour
{

    public static float progress;

    private void Start()
    {
        progress = PlayerPrefs.GetFloat("Game Progress");
    }

    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("progress");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        PlayerPrefs.SetFloat("Game Progress", progress);
    }
}
