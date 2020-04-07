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
    //The distance the player must be to the final boss to initiate the waves spawning.
    public float minDistanceToPlayer = 10f;

    public int wavesToSpawn = 3;
    public int maxRounds = 3;
    private int currentWave = 0;
    private int currentEnemyCount = 0;

    private bool playerInRange = false;
    private bool waveStarted = false;
    private bool waveFinished = false;
    private bool roundOver = false;

    public float timeBetweenWaves = 3f;
    private float currentTime = 0f;

    private int currentRound = 0;

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
            print(dist);
            return;
        }

        //Start the wave
        if (!waveStarted)
        {
            //If roundOver is true, make it false because there are still waves to spawn.
            if (roundOver)
                roundOver = false;

            StartWave();
        }
        //If the wave has started and the player has killed all of the spawned enemies, 
        //set finished var to true.
        else if (waveStarted && !waveFinished && currentEnemyCount <= 0)
        {
            print("WAVE " + (currentWave + 1) + " CLEARED!");

            waveFinished = true;
        }

        //Now that the wave is finished, we begin waiting for timeBetweenWaves until the next wave comes.
        if (waveFinished && currentTime < timeBetweenWaves)
        {
            currentTime += Time.deltaTime;
        }
        else if (currentTime >= timeBetweenWaves && !roundOver)
        {
            //Finalize the current wave and start the new one.
            FinalizeWave();
        }
    }

    void StartWave()
    {
        //TODO: BLOW WHISTE HERE
        print("WHISTLE BLOW!: Round #" + (currentRound + 1) + "     Wave #" + (currentWave + 1));
        waveStarted = true;

        //Get an array of all of the spawners in the scene.
        GameObject[] spawnerObjects = GameObject.FindGameObjectsWithTag("Minion Spawner");
        MinionSpawner[] spawners = new MinionSpawner[spawnerObjects.Length];
        for (int i = 0; i < spawnerObjects.Length; ++i)
        {
            spawners[i] = spawnerObjects[i].GetComponent<MinionSpawner>();
            //If the spawner is not supposed to be run this round, skip to the next available spawner.
            if (spawners[i].roundActive != (currentRound + 1))
                continue;

            spawners[i].SpawnMinions();
            currentEnemyCount += spawners[i].GetEnemyCount();
        }
    }

    public void DecreaseMinionCount()
    {
        //If the round is over or the wave hasn't started yet, don't decrease the enemy count.
        if (roundOver || !waveStarted)
            return;
        currentEnemyCount--;
        print("Current Enemy Count: " + currentEnemyCount);
    }

    void FinalizeWave()
    {
        currentTime = 0f;
        if (currentEnemyCount != 0)
            currentEnemyCount = 0;

        //If we reached the max number of waves for the level, perform transition operations.
        currentWave++;
        if (currentWave >= wavesToSpawn)
        {
            //Increase currentRound to the round that just ended.
            currentRound++;
            if (currentRound >= maxRounds)
            {
                roundOver = true;
                print("ALL ROUNDS HAVE BEEN COMPLETED!");
                //TODO: Make the final boss jump down and attack the player.
                return;
            }
            currentWave = 0;
            //TODO: Make boss run away AND fans turn on and stuff.
            print("THIS ROUND IS OVER. THE BOSS HAS MOVED!");
            MoveBossPosition();
            TurnOnInteractables();
            playerInRange = false;
            roundOver = true;
        }

        waveFinished = false;
        waveStarted = false;
    }

    //Interactables with the tag "'Round ' + currentRound'" will be turned on after the final boss moves.
    void TurnOnInteractables()
    {
        string tag = "Round " + currentRound;
        GameObject[] interactableObjects = GameObject.FindGameObjectsWithTag(tag);
        InteractableBehaviour[] interactables = new InteractableBehaviour[interactableObjects.Length];

        for (int i = 0; i < interactables.Length; ++i)
        {
            interactables[i] = interactableObjects[i].GetComponent<InteractableBehaviour>();
            interactables[i].PowerOn();
        }
    }

    void MoveBossPosition()
    {
        string name = "Boss Pos " + (currentRound + 1);
        Transform newPos = GameObject.Find(name).transform;

        transform.position = newPos.position;
    }
}