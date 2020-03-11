/*****************************************************************************
// File Name : WinAreaBehaviour
// Author : Kyle Grenier
// Creation Date : March 10, 2020
//
// Brief Description : When the player walks in here, finish the level.
*****************************************************************************/

using UnityEngine;

public class WinAreaBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameControllerScript.instance.FinishLevel();
        }
    }
}
