/*****************************************************************************
// File Name : TurretScript.cs
// Author : Connor Dunn
// Creation Date : February 17, 2020
//
// Brief Description :  A functional turret that activates when the player 
//                      is in range, and follows the player while shooting 
//                      bullets.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public bool facingRight = false;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 playerPosition = collision.gameObject.transform.position;
            Vector2 delta = transform.position - playerPosition;

            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;


            if (facingRight == false)
            {
                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = rot;
            }
            else
            {
                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 180));
                transform.rotation = rot;
            }
            
            Invoke("shoot", 2f);
        }
    }

    public float timeBetweenShots = 1f;
    private float validShotTime;
    void shoot()
    {
        if(Time.time >= validShotTime)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            validShotTime = Time.time + timeBetweenShots;
        }
        
    }
}
