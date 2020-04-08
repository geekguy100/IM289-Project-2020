/*****************************************************************************
// File Name : WinAreaBehaviour
// Author : Kyle Grenier (20%), Robert Lee (80%)
// Creation Date : March 10, 2020
//
// Brief Description : When the player walks in here, finish the level and
                       update their progress.
*****************************************************************************/

using UnityEngine;

public class WinAreaBehaviour : MonoBehaviour
{

    public float increaseprogress;
    public GameObject WinScreen;

    private void Start()
    {
        WinScreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameControllerScript.instance.FinishLevel();
            Time.timeScale = 0;
            ProgressCheck.progress += increaseprogress;
            WinScreen.SetActive(true);
        }
    }
}
