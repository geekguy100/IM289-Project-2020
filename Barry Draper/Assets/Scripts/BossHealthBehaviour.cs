/*****************************************************************************
// File Name : BossHealthBehaviour
// Author : Kyle Grenier
// Creation Date : April 09, 2020
//
// Brief Description : Controls the final boss's health.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBehaviour : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;
    public GameObject levelCompleteCanvas;

    //private Animator anim;

    public float hitAnimationTime = 1f;

    [HideInInspector]
    public bool beenHit = false;
    [HideInInspector]
    public bool beenKilled = false;

    void Awake()
    {
        //anim = GetComponent<Animator>();

        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        GetComponent<AudioSource>().Play();

        if (currentLives <= 0)
        {
            //Make sure this doesn't run twice.
            if (beenKilled)
                return;

            //anim.SetBool("IsKilled", true);
            beenKilled = true;
            GameControllerScript.instance.OnGameComplete(levelCompleteCanvas);
            //PLAY AUDIO EFFECT
            Destroy(gameObject, 1f);
            return;
        }

        //anim.SetBool("IsHit", true);
        beenHit = true;
        Invoke("RemoveHitAnimation", hitAnimationTime);
    }

    private void RemoveHitAnimation()
    {
        //anim.SetBool("IsHit", false);
        beenHit = false;
    }
}