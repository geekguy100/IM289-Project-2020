/*****************************************************************************
// File Name : BossShootingBehaviour
// Author : Kyle Grenier
// Creation Date : April 01, 2020
//
// Brief Description : Controls the boss's shooting
*****************************************************************************/

using UnityEngine;

public class BossShootingBehaviour : MonoBehaviour
{
    public Transform bulletSpawnPos;
    public GameObject bullet;

    public float timeBetweenShots = 0.5f;
    private float currentShootTime = 0f;
    private bool canShoot = true;

    private Animator anim;

    BossHealthBehaviour healthBehaviour;

    private AudioController audioController;

    private void Awake()
    {
        healthBehaviour = GetComponent<BossHealthBehaviour>();
        anim = GetComponent<Animator>();
        audioController = GetComponentInChildren<AudioController>();
    }

    public void HandleShooting()
    {
        if (!canShoot || healthBehaviour.beenHit || healthBehaviour.beenKilled)
        {
            anim.SetBool("IsShooting", false);
            return;
        }

        Instantiate(bullet, bulletSpawnPos.position, bulletSpawnPos.rotation);
        audioController.PlayClip(AudioController.BossSFX.shoot);
        anim.SetBool("IsShooting", true);
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