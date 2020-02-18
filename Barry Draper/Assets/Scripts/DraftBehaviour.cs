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

    [Tooltip("The force to be applied, either applied instantaneouly" +
        " or over time.")]
    public float force;


    [Tooltip("If the draft should provide a suddent burst of force. " +
        "               Otherwise, it will apply the force over time.")]
    public bool instantaneous = false;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (instantaneous)
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!instantaneous)
        {
            if (gameObject.tag == "up" && player.GetComponent<PlayerController>
                                                        ().umbrellaUp == true)
            {
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(direction * force, ForceMode2D.Force);
            }
        }
    }
}
