/*****************************************************************************
// File Name : BulletBehaviour.cs
// Author : Connor Dunn
// Creation Date : February 19, 2020
//
// Brief Description : Bullets fly forward and impact the player and turrets upon reflection from shield.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Main Attributes")]
    public float bulletSpeed = 5;
    public float destroyTime = 10f;

    [Header("Damage Attributes")]
    public int damageDealt = 1;

    private Rigidbody2D rb;
    private Vector3 direction;

    private bool canHurtMinion = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * bulletSpeed);
    }

    //private void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.CompareTag("Player"))
    //    {
    //        //Note: Player hit SFX is handled in the GameControllerScript.
    //        GameControllerScript.instance.RemoveLivesFromPlayer(1);
    //    }
    //    else if (col.gameObject.CompareTag("Minion"))
    //    {
    //        col.gameObject.GetComponent<MinionHealthBehaviour>().TakeDamage(1);
    //    }
    //    else
    //    {
    //        //TODO: Play a bullet destroyed SFX.
    //        print("Collided with an unknown collider: " + col.gameObject.name);
    //    }

    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Umbrella"))
        {
            //TODO: Play a blocked SFX.
            direction = -direction;

            //Once the bullet bounces off of the umbrella, it can hurt minions.
            //This is to prevent minions from firing at each other.
            canHurtMinion = true;
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            //Note: Player hit SFX is handled in the GameControllerScript.
            GameControllerScript.instance.RemoveLivesFromPlayer(1);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Minion"))
        {
            //Play minion hurt SFX.
            if (canHurtMinion)
            {
                col.gameObject.GetComponent<MinionHealthBehaviour>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.CompareTag("Turret"))
        {
            //TODO: Play a metalic SFX to notify of turret damage infliction.
            col.gameObject.GetComponent<TurretHealthBehaviour>().TakeDamage(damageDealt);
            Destroy(gameObject);
        }
        else
        {
            //TODO: Play a bullet destroyed SFX.
            //print("Collided with another trigger: " + col.gameObject.name);
            Destroy(gameObject);
        }
    }
}