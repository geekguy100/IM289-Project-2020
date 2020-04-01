/*****************************************************************************
// File Name : FinalBossBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : Code that controls the mechanics of the final boss, including spawning minion waves and transitioning to the next stage.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossBehaviour : MonoBehaviour
{
    private Transform player;
    public float minDistanceToPlayer = 10f;

    public int wavesToSpawn = 3;
    private int currentWave = 0;
    private int currentEnemyCount = 0;

    private bool playerInRange = false;
    private bool waveStarted = false;
    private bool waveFinished = false;

    public float timeBetweenWaves = 3f;
    private float currentTime = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        //Make sure the player is in range before doing anything.
        //Only for the inital wave so the player finds the boss before the first wave starts.
        if (!playerInRange)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            playerInRange = dist < minDistanceToPlayer;
            return;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            DecreaseMinionCount();
        }

        //Start the wave
        if (!waveStarted)
        {
            StartWave();
        }
        //If the wave has started and the player has killed all of the spawned enemies, 
        //set finished var to true.
        else if (waveStarted && !waveFinished && currentEnemyCount <= 0)
        {
            print("WAVE " + (currentWave + 1) + " CLEARED!");
            currentEnemyCount = 0; //Just in case by some random coincedence this goes below 0.

            waveFinished = true;
        }

        //Now that the wave is finished, we begin waiting for timeBetweenWaves until the next wave comes.
        if (waveFinished && currentTime < timeBetweenWaves)
        {
            currentTime += Time.deltaTime;
        }
        else if (currentTime >= timeBetweenWaves)
        {
            //Finalize the current wave and start the new one.
            FinalizeWave();
        }
    }

    void StartWave()
    {
        //TODO: BLOW WHISTE HERE
        print("WHISTLE BLOW!: Wave #" + (currentWave + 1));
        waveStarted = true;

        //Get an array of all of the spawners in the scene.
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Minion Spawner");
        MinionSpawner[] spawners = new MinionSpawner[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; ++i)
        {
            spawners[i] = spawnerObjects[i].GetComponent<MinionSpawner>();
            spawners[i].SpawnMinions();
            currentEnemyCount += spawners[i].GetEnemyCount();
        }
    }

    public void DecreaseMinionCount()
    {
        currentEnemyCount--;
        print("Current Enemy Count: " + currentEnemyCount);
    }

    void FinalizeWave()
    {
        currentTime = 0f;
        //If we reached the max number of waves for the level, perform transition operations.
        currentWave++;
        if (currentWave >= wavesToSpawn)
        {
            //TODO: Make boss run away AND fans turn on and stuff.
            print("THIS ROUND IS OVER. THE BOSS HAS MOVED!");
            return;
        }

        waveFinished = false;
        waveStarted = false;
    }
}