/*****************************************************************************
// File Name : TurretHealthBehaviour
// Author : Kyle Grenier
// Creation Date : March 07, 2020
//
// Brief Description : Controls the life of a turret. Life is affected by bullets hitting the turret.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealthBehaviour : MonoBehaviour
{
    public int maxLives = 5;
    private int currentLives;

    private void Awake()
    {
        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        if (currentLives <= 0)
        {
            //TODO: Add particle effects or maybe an animation of the turret falling apart.
            //TODO: Add SFX.
            Destroy(gameObject);
        }
    }
}