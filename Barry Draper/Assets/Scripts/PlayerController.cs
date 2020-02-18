/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Kyle Grenier
// Creation Date :     February 8, 2020
//
// Brief Description : Script that translates player input into actual movement
   of the character.
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

    [Header("Game Objects")]                                /*CD*/
    [Tooltip("The sprite for the player's umbrella.")]      /*CD*/
    public GameObject umbrellaObject;                       /*CD*/

    public static string umbrellaOrientation;               /*CD*/
    
    //Is the players umbrella is activated or not
    private bool umbrella = false;

    [Header("Rotations of the umbrella")]
    Quaternion up = Quaternion.Euler(new Vector3(0, 0, 0));
    Quaternion right = Quaternion.Euler(new Vector3(0, 0, 270));
    Quaternion down = Quaternion.Euler(new Vector3(0, 0, 180));
    Quaternion left = Quaternion.Euler(new Vector3(0, 0, 90));

    [Header("Bools that tell what direction the Umbrella is in")]
    public bool umbrellaUp = true;
    public bool umbrellaDown = false;
    public bool umbrellaRight = false;
    public bool umbrellaLeft = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        umbrellaOrientation = "idle";                       /*CD*/
    }

    void Update()
    {
        if (isAlive)
        {
            //Updating the newPos x value (where the player will move to)
            //based on if the player is grounded or not.
            float xMov = Input.GetAxis("Horizontal");

            //Checking if the player is grounded.
            isGrounded = Physics2D.Linecast(transform.position,
                        groundPosition.position, whatIsGround);

            //Updating the x value accordingly.
            if (isGrounded)
                newPos.x = xMov * moveSpeed;
            else
                newPos.x = xMov * airSpeed;

            ActivateUmbrella();
            PointUmbrella();
        }
    }

    private void FixedUpdate()
    {
        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;

        rb.position = pos;
    }

    /// <summary>
    /// If the player presses the space bar, the umbrella will open or close,
    /// the swap bool will be swaped in the swap function which is invoked
    /// after the player presses spacebar.
    /// Connor Riley
    /// </summary>
    void ActivateUmbrella()
    {
        if(Input.GetKeyDown(KeyCode.Space) && umbrella == false)
        {
            rb.mass = 1;
            rb.gravityScale = 0.5f;
            Invoke("swap", 1f);
        }

        if(Input.GetKeyDown(KeyCode.Space) && umbrella == true)
        {
            rb.mass = 1.5f;
            rb.gravityScale = 0.75f;
            Invoke("swap", 1f);
        }
    }

    /// <summary>
    /// flips the umbrella bool to whatever it isn't.
    /// Connor Riley
    /// </summary>
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

    /// <summary>
    /// Switches the rotation of the players umbrella by pressing the
    /// up, down, left, and right arrow keys.
    /// Connor Riley
    /// </summary>
    void PointUmbrella()
    {
        if (Input.GetButton("UmbrellaUp"))
        {
            umbrellaObject.transform.rotation = up;
            umbrellaUp = true;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = false;
        }

        if(Input.GetButton("UmbrellaRight"))
        {
            umbrellaObject.transform.rotation = right;
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = true;
        }

        if(Input.GetButton("UmbrellaDown"))
        {
            umbrellaUp = false;
            umbrellaDown = true;
            umbrellaLeft = false;
            umbrellaRight = false;
            umbrellaObject.transform.rotation = down;
        }

        if (Input.GetButton("UmbrellaLeft"))
        {
            umbrellaObject.transform.rotation = left;
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = true;
            umbrellaRight = false;
        }
    }

    public static bool isAlive = true;
    public static bool livesChanged = false;

    /// <summary>
    /// When the player collides with objects. In this case, spikes.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the collided object is a spike trap, reduce player's lives
        //and update UI.
        if(collision.gameObject.tag == "Spikes" && 
                  !GameControllerScript.invincible)
        {
            GameControllerScript.playerLives -= 1;
            livesChanged = true;
            GameControllerScript.invincible = true;
        }
    }
}
