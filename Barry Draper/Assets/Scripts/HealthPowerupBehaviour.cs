/*****************************************************************************
// File Name : HealthPowerupBehaviour
// Author : Kyle Grenier
// Creation Date : April 08, 2020
//
// Brief Description : Controls how much health the powerup will give the player. Also controls rotation of the object.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerupBehaviour : MonoBehaviour
{
    public int health = 1;
    public float rotationSpeed = 500;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //If lives have been awarded, destroy the power up.
            if (GameControllerScript.instance.AwardLives(health))
                Destroy(gameObject);
        }
    }
}
