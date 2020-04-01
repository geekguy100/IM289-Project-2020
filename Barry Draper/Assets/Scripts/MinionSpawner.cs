/*****************************************************************************
// File Name : MinionSpawner
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : Spawns the minions for the final boss battle.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{
    //public GameObject minion;
    [SerializeField]
    private int numberOfMinions = 5;

    public int GetEnemyCount()
    {
        return numberOfMinions;
    }
}