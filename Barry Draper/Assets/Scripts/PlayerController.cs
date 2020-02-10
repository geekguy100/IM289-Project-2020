/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Kyle Grenier
// Creation Date :     February 8, 2020
//
// Brief Description : Script that translates player input into actual movement of the character.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Player Movement")]
    [Tooltip("Movement speed of character on ground.")]
    public float moveSpeed;
    [Tooltip("Movement speed of character while in the air.")]
    public float airSpeed;
    private Vector2 newPos;

    [Header("Movement Mechanics")]
    public Transform groundPosition;
    public LayerMask whatIsGround;
    private bool isGrounded = false;

    //Is the players umbrella is activated or not
    private bool umbrella = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Updating the newPos x value (where the player will move to)
        //based on if the player is grounded or not.
        float xMov = Input.GetAxis("Horizontal");

        //Checking if the player is grounded.
        isGrounded = Physics2D.Linecast(transform.position, groundPosition.position, whatIsGround);

        //Updating the x value accordingly.
        if (isGrounded)
            newPos.x = xMov * moveSpeed;
        else
            newPos.x = xMov * airSpeed;

        ActivateUmbrella();

    }

    private void FixedUpdate()
    {
        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;

        rb.position = pos;
    }

    void ActivateUmbrella()
    {
        if(Input.GetKeyDown(KeyCode.Space) && umbrella == false)
        {
            rb.gravityScale = 0.5f;
            Invoke("swap", 1f);
        }

        if(Input.GetKeyDown(KeyCode.Space) && umbrella == true)
        {
            rb.gravityScale = 0.75f;
            Invoke("swap", 1f);
        }
    }

    void swap()
    {

        if(umbrella == false)
        {
            umbrella = true;
        }
        else 
        {
            umbrella = false;
        }
    }
}
