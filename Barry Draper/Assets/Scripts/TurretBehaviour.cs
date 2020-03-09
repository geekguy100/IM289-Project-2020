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

public class TurretBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool facingRight = false;

    [Header("Shooting Attributes")]
    public GameObject bullet; //Bullet turret will shoot.
    public Transform bulletSpawnPos;

    public float maxDistance = 5f; //The max distance the target can be (AKA the length of the ray).
    public LayerMask shootLayer; //The layer that includes things to shoot at (i.e., the player).

    private Transform target = null;

    private Vector2 direction; //Direction turret should shoot in.
    public float rotateSpeed = 5f;

    private bool canShoot = false;
    private float currentShootTime;
    private float currentWarmupTime;
    public float delayBetweenShots = 0.5f;
    public float warmUpTime = 1f;


    [Header("Rotation Attributes")]
    public float minRotationAngle = -45f;
    public float maxRotationAngle = 45f;

    private Quaternion defRot;

    private void Start()
    {
        defRot = transform.parent.localRotation;
    }

    private void Update()
    {
        if (facingRight)
            direction = transform.parent.right;
        else
            direction = -transform.parent.right;

        //Debug.DrawRay(bulletSpawnPos.position, direction * maxDistance, Color.red);

        RaycastHit2D foundTarget = Physics2D.Raycast(bulletSpawnPos.position, direction, maxDistance, shootLayer);
        if (foundTarget)
        {
            //Rotate head towards target
            target = foundTarget.transform;

            Vector2 delta;
            if (facingRight)
                delta = target.position - bulletSpawnPos.position;
            else
                delta = bulletSpawnPos.position - target.position;

            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            angle = Mathf.Clamp(angle, minRotationAngle, maxRotationAngle);

            Quaternion rot = Quaternion.Euler(new Vector3(0,0,angle));

            if (Mathf.Abs(Quaternion.Angle(transform.parent.rotation, rot)) > 0.05f)
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rot, Time.deltaTime * rotateSpeed);


            //Shooting:
            //First, let the turret "warmup" with the warmup timer.
            if (!canShoot)
            {
                //TODO: play an in-range SFX.
                currentWarmupTime += Time.deltaTime;

                if (currentWarmupTime >= warmUpTime)
                {
                    canShoot = true;
                    currentWarmupTime = 0f;
                }
            }

            //Once the turret has warmed up, shoot following the appropriate delay between shots.
            if (canShoot)
            {
                currentShootTime += Time.deltaTime;
                if (currentShootTime >= delayBetweenShots)
                {
                    Shoot();
                }
            }
        }
        //Rotating back to the default rotation.
        else if (Mathf.Abs(Quaternion.Angle(transform.parent.rotation, defRot)) > 0.05f)
        {
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, defRot, Time.deltaTime * rotateSpeed);

            if (canShoot)
            {
                canShoot = false;
                currentShootTime = 0f;
            }
        }
    }

    void Shoot()
    {
        Vector3 shootEuler = transform.parent.rotation.eulerAngles;

        if (!facingRight)
            shootEuler.z += 180;

        Quaternion shootRot = Quaternion.Euler(shootEuler);

        //TODO: Play a gun SFX.
        Instantiate(bullet, bulletSpawnPos.position, shootRot);
        currentShootTime = 0f;
    }
}