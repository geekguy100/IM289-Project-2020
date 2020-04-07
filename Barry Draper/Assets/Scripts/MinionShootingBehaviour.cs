/*****************************************************************************
// File Name : MinionShootingBehaviour
// Author : Kyle Grenier
// Creation Date : April 01, 2020
//
// Brief Description : Controls the minions' shooting
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionShootingBehaviour : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public GameObject bullet;
 
    public float timeBetweenShots = 0.5f;
    private float currentShootTime = 0f;
    private bool canShoot = true;

    public void HandleShooting()
    {
        if (!canShoot) return;

        Instantiate(bullet, bulletSpawnPos.position, bulletSpawnPos.rotation);
        canShoot = false;
    }

    private void Update()
    {
        //Updating whether or not the minion can shoot.
        if (!canShoot)
        {
            currentShootTime += Time.deltaTime;
        }

        if (currentShootTime >= timeBetweenShots)
        {
            canShoot = true;
            currentShootTime = 0f;
        }
    }
}