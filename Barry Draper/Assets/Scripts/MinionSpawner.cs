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
    public GameObject minion;
    //The number of minions to spawn.
    [SerializeField] private int numberOfMinions = 5;
    //The number of minions currently spawned.
    private int currentNum = 0;

    //The time between each minion spawn in seconds.
    public float timeBetweenSpawns = 1f;



    public int GetEnemyCount()
    {
        return numberOfMinions;
    }

    public void SpawnMinions()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (currentNum < numberOfMinions)
        {
            Instantiate(minion, transform.position, Quaternion.identity);
            currentNum++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        currentNum = 0;
    }
}