/*****************************************************************************
// File Name : MinionHealthBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : Controls minions' health.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealthBehaviour : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    private Animator anim;

    public float hitAnimationTime = 1f;

    [HideInInspector]
    public bool beenHit = false;
    [HideInInspector]
    public bool beenKilled = false;

    private AudioController audioController;

    void Awake()
    {
        anim = GetComponent<Animator>();
        audioController = GetComponentInChildren<AudioController>();
        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        if (currentLives <= 0)
        {
            //Make sure this doesn't run twice.
            if (beenKilled)
                return;

            anim.SetBool("IsKilled", true);
            beenKilled = true;

            //PLAY AUDIO EFFECT
            GameObject.FindObjectOfType<FinalBossBehaviour>().DecreaseMinionCount();
            audioController.PlayClip(AudioController.MinionSFX.minionDie);
            Destroy(gameObject, 1f);
            return;
        }

        audioController.PlayClip(AudioController.MinionSFX.minionHit);
        anim.SetBool("IsHit", true);
        beenHit = true;
        Invoke("RemoveHitAnimation", hitAnimationTime);
    }

    private void RemoveHitAnimation()
    {
        anim.SetBool("IsHit", false);
        beenHit = false;
    }
}
