/*****************************************************************************
// File Name : BulletScript.cs
// Author : Connor Dunn
// Creation Date : February 19, 2020
//
// Brief Description : Bullets fly forward and impact the player.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : TurretScript
{
    public float bulletSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.right * Time.deltaTime * bulletSpeed);
        if(facingRight == false)
        {
            transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
        }
        else if(facingRight == true)
        {
            transform.Translate(Vector3.left * Time.deltaTime * bulletSpeed);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            GameControllerScript.playerLives--;
        }
    }
}