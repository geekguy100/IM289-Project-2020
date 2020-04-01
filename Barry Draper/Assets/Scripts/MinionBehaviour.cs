/*****************************************************************************
// File Name : MinionBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : Controls the beaviour for the minions such as following the player.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionBehaviour : MonoBehaviour
{
    private Transform player;
    private Vector2 newPos;
    public Vector2 movementSpeed = new Vector2(2, 4);
    private float assignedMovementSpeed;

    public Vector2 distanceToPlayer = new Vector2(5f, 10f);
    private float assignedDistanceToPlayer;
    private float currentDistanceToPlayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        assignedDistanceToPlayer = Random.Range(distanceToPlayer.x, distanceToPlayer.y);
        assignedMovementSpeed = Random.Range(movementSpeed.x, movementSpeed.y);
    }

    private void Update()
    {
        currentDistanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (currentDistanceToPlayer <= assignedDistanceToPlayer)
        {
            if (newPos.x != 0)
            {
                newPos.x = 0;
                assignedDistanceToPlayer = Random.Range(distanceToPlayer.x, distanceToPlayer.y);
            }

            //HANDLE SHOOTING()

            return; //Returning so we don't move the minion while he is supposed to be stationary.
        }

        //Calculating the direction and velocity for the minion.
        Vector2 direction = (player.position - transform.position).normalized;
        newPos.x = direction.x * assignedMovementSpeed;
    }

    void FixedUpdate()
    {
        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;
        rb.position = pos;
    }
}