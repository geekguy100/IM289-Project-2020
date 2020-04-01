/*****************************************************************************
// File Name : MinionHealthBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : ADD BRIEF DESCRIPTION HERE
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealthBehaviour : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    void Awake()
    {
        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives < 0)
        {
            //PLAY AUDIO EFFECT
            GameObject.FindObjectOfType<FinalBossBehaviour>().DecreaseMinionCount();
            Destroy(gameObject);
        }
    }
}
