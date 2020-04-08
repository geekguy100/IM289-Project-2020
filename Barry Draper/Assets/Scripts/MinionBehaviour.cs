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

    private MinionShootingBehaviour shootingBehaviour;
    private MinionHealthBehaviour healthBehaviour;

    private Rigidbody2D rb;

    private bool facingRight = false;

    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shootingBehaviour = GetComponent<MinionShootingBehaviour>();
        healthBehaviour = GetComponent<MinionHealthBehaviour>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        assignedDistanceToPlayer = Random.Range(distanceToPlayer.x, distanceToPlayer.y);
        assignedMovementSpeed = Random.Range(movementSpeed.x, movementSpeed.y);
    }

    private void Update()
    {
        //If the enemy has been hit or killed, don't make him move or shoot -- wait until he has recovered.
        if (healthBehaviour.beenHit || healthBehaviour.beenKilled)
            return;

        //Calculating distance and direction towards the player.
        currentDistanceToPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 direction = (player.position - transform.position).normalized;

        //Handle flipping here.
        HandleFlipping(direction);

        if (currentDistanceToPlayer > assignedDistanceToPlayer)
        {
            anim.SetBool("IsRunning", true);
            //Calculating the velocity for the minion.
            newPos.x = direction.x * assignedMovementSpeed;
        }
        else
        {
            anim.SetBool("IsRunning", false);
            if (newPos.x != 0)
            {
                newPos.x = 0;
            }

            //Handle shooting. Time between shots, bullet prefab, etc., are all in the MinionShootingBehaviour script.
            shootingBehaviour.HandleShooting();
        }
    }

    private void HandleFlipping(Vector2 direction)
    {
        if (direction.x < 0 && facingRight)
        {
            Flip();
        }
        else if (direction.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += 180;

        //Invert the boolean.
        facingRight = !facingRight;

        transform.rotation = Quaternion.Euler(rot);
    }
    
    void FixedUpdate()
    {
        //Don't move the minion if he's been hit or killed.
        if (healthBehaviour.beenHit || healthBehaviour.beenKilled)
            return;
        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;
        rb.position = pos;
    }
}