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

    private void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D rb = col.GetComponent<Rigidbody2D>();

        if (col.gameObject.CompareTag("Player"))
        {
            PlayerController pc = col.gameObject.GetComponent<PlayerController>();

            if (gameObject.tag == "up")
            {
                if (pc.umbrellaUp == true)
                {
                    AddForce(rb, 2f);
                }
            }
            else if (gameObject.tag == "right")
            {
                if (pc.umbrellaRight == true)
                {
                    AddForce(rb, 2f);
                }
            }
            else if (gameObject.tag == "left")
            {
                if (pc == true)
                {
                    AddForce(rb, 2f);
                }
            }
        }
        else //if a rigidbody other than the player enters the draft
        {
            AddForce(rb, 1f);
        }
    }

    void AddForce(Rigidbody2D rb, float modifier = 1)
    {
        rb.AddForce(direction * force * modifier, ForceMode2D.Force);
    }
}
