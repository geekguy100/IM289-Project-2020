/*****************************************************************************
// File Name :         DraftBehaviour.cs
// Author :            Kyle Grenier
// Creation Date :     February 8, 2020
//
// Brief Description : Controls the behaviour of drafts/wind in the game, 
                       including direction and force applied to objects.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftBehaviour : MonoBehaviour
{
    public Vector2 direction;

    public float force;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if (!rb)
            return;

        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(CircleCollider2D))
                return;

            PlayerController pc = col.gameObject.GetComponent<PlayerController>();

            //If the player's umbrella is NOT open, don't bother applying
            //a force to him.
            if (!pc.umbrella)
                return;

            //Making sure the direction of the force and the direction of the
            //player's umbrella align.
            if (gameObject.tag == "up" && pc.umbrellaUp)
            {
                ApplyForce(rb);
            }
            else if (gameObject.tag == "right" && pc.umbrellaRight)
            {
                ApplyForce(rb);
            }
            else if (gameObject.tag == "left" && pc.umbrellaLeft)
            {
                ApplyForce(rb);
            }
        }
        else if (!col.gameObject.CompareTag("Umbrella")) //if some other rigidbody enters the draft
        {
            ApplyForce(rb);
        }
    }

    private void ApplyForce(Rigidbody2D rb)
    {
        rb.AddForce(direction * force, ForceMode2D.Force);
        if (audioSource.isPlaying)
            return;
        else
            audioSource.Play();
    }
}
