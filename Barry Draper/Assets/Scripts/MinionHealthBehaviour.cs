/*****************************************************************************
// File Name : MinionHealthBehaviour
// Author : Kyle Grenier
// Creation Date : March 31, 2020
//
// Brief Description : ADD BRIEF DESCRIPTION HERE
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHealthBehaviour : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    private Animator anim;

    public float hitAnimationTime = 0.5f;

    public static bool beenHit = false;
    public static bool beenKilled = false;

    void Awake()
    {
        anim = GetComponent<Animator>();

        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        anim.SetBool("IsHit", true);
        beenHit = true;
        Invoke("RemoveHitAnimation", hitAnimationTime);

        if (currentLives <= 0)
        {
            anim.SetBool("IsKilled", true);
            beenKilled = true;

            //PLAY AUDIO EFFECT
            GameObject.FindObjectOfType<FinalBossBehaviour>().DecreaseMinionCount();
            Destroy(gameObject);

            Invoke("DestroyEnemy", 0.5f);
        }
    }

    public void RemoveHitAnimation()
    {
        anim.SetBool("IsHit", false);
        beenHit = false;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
