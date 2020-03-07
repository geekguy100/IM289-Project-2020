/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Connor Riley (57%):
                            Implemented umbrella behaviours such as rotation,
                            checking which direction the umbrella is in,
                            changing air speed depending on what orientation
                            the umbrella is in the air.
                       Kyle Grenier (28%):
                             Implemented character movement, getting
                             important components on the character.
                       Connor Dunn (15%):
                              Implemented spike interaction with the player,
                              as well as animation functionality and sprite
                              changes to reflect umbrella directions.
// Creation Date :     February 8, 2020
//
// Brief Description : Script that translates player input into actual movement
   of the character.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnorPlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Player Movement")]
    [Tooltip("Movement speed of character on ground.")]
    public float moveSpeed;
    [Tooltip("Movement speed of character while in the air.")]
    public float airSpeed;
    [HideInInspector]
    public Vector2 newPos;
    public float minYVel = -4.9f;
    [Header("Dashing")]
    public float dashForce = 5.0f;
    public float dashTime = 1f;
    public float dashCooldownTime = 0.5f;
    [SerializeField]
    private bool canDash = true;
    [Header("Movement Dependencies")]
    public Transform groundPosition;
    public LayerMask whatIsGround;
    private bool isGrounded = false;
    private bool facingRight = true;

    [Header("Game Objects")]                                /*CD*/
    [Tooltip("The sprite for the player's umbrella.")]      /*CD*/
    public GameObject umbrellaObject;
    //public GameObject playerUmbrella;


    //Is the players umbrella is activated or not
    [HideInInspector]
    public bool umbrella = false;

    [Header("Rotations of the umbrella")]
    Quaternion up = Quaternion.Euler(new Vector3(0, 0, 0));
    Quaternion right = Quaternion.Euler(new Vector3(0, 0, 270));
    Quaternion down = Quaternion.Euler(new Vector3(0, 0, 180));
    Quaternion left = Quaternion.Euler(new Vector3(0, 0, 90));

    public Sprite[] spriteArray = new Sprite[8];
    /* 0 = Col Right    |   4 = Act Right
     * 1 = Col Up       |   5 = Act Up
     * 2 = Col Left     |   6 = Act Left
     * 3 = Col Down     |   7 = Act Down
    */

    GameObject umbrellaSprite;

    [Header("Bools that tell what direction the Umbrella is in")]
    [HideInInspector] public bool umbrellaUp = true;
    [HideInInspector] public bool umbrellaDown = false;
    [HideInInspector] public bool umbrellaRight = false;
    [HideInInspector] public bool umbrellaLeft = false;

    [Header("Player Interaction Attributes")]
    //The max distance the player must be from an object to be able to pick it up.
    public float grabDistance = 1f;

    public LayerMask interactableLayer;
    public Transform objHoldPos;
    public Transform rayStart;
    private Rigidbody2D theObjectInRange;

    //The offset of where the ray will be casted in relation to the player's position.
    public Vector3 grabOffset = Vector3.zero;

    private bool objectGrabbed = false;

    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    //Used to visualize the box cast used to check if the player is grounded. KG
    float boxCastYScale = 0.05f;
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the groundPosition's position. KG
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(groundPosition.position, new Vector2(transform.localScale.x, boxCastYScale));
    }






    void Update()
    {
        //If the player is not alive don't bother running the code in this function.
        if (!GameControllerScript.instance.playerAlive)
            return;

        //Updating the newPos x value (where the player will move to)
        //based on if the player is grounded or not. KG
        float xMov = Input.GetAxis("Horizontal");

        //Updating player's direction (which direction he is facing).
        if ((xMov > 0 && !facingRight) || (xMov < 0 && facingRight))
            Flip();


        //Checking if the player is grounded.
        isGrounded = Physics2D.BoxCast(groundPosition.position, new Vector2(Mathf.Abs(transform.localScale.x), boxCastYScale), 0f, Vector3.zero, 0f, whatIsGround);

        //Updating the x value accordingly.
        if (isGrounded)
            newPos.x = xMov * moveSpeed;
        else
            newPos.x = xMov * airSpeed;

        anim.SetFloat("xMov", xMov);
        anim.SetBool("isGrounded", isGrounded);

        ActivateUmbrella();
        PointUmbrella();




        //Checking to see if the player is in range of a grabbable object.
        RaycastHit2D objectInRange = Physics2D.Raycast(rayStart.position, transform.right, grabDistance, interactableLayer);
        Debug.DrawRay(rayStart.position, transform.right * grabDistance, Color.green);

        if (objectInRange || objectGrabbed)
        {
            if (objectGrabbed)
            {
                HandleGrabbing(theObjectInRange);
            }
            else if (!objectGrabbed)
            {
                theObjectInRange = objectInRange.transform.GetComponent<Rigidbody2D>();
                HandleGrabbing(theObjectInRange);
            }
        }

    }

    private float minYVelWithUmbrella = -4.9f;
    private float minYVelWithoutUmbrella = -15f;

    private void FixedUpdate()
    {
        //If the player is not alive don't bother running the code in this function.
        if (!GameControllerScript.instance.playerAlive)
            return;

        //Moving the rigidbody along x-axis.
        Vector2 pos = rb.position;
        pos.x += newPos.x * Time.fixedDeltaTime;

        //Clamp the y-axis velocity to prevent the player from falling too fast with the umbrella in hand and facing up.
        if (umbrella && umbrellaUp)
        {
            Vector2 vel = rb.velocity;
            vel.y = Mathf.Clamp(vel.y, minYVelWithUmbrella, 900f);
            rb.velocity = vel;
        }
        //if the player has the umbrella open in some other direction or not open at all
        else
        {
            Vector2 vel = rb.velocity;
            vel.y = Mathf.Clamp(vel.y, minYVelWithoutUmbrella, 900f);
            rb.velocity = vel;
        }

        //If the player is grounded AND the x velocity does NOT equal 0 AND the player CANNOT dash, change it to 0.
        if (isGrounded && rb.velocity.x != 0 && !canDash)
        {
            Vector2 vel = rb.velocity;
            vel.x = 0;
            rb.velocity = vel;

            //print("Changed the x velocity");
        }

        rb.position = pos;
    }

    /// <summary>
    /// Flip the player transform horizontally so they are facing in the direction they are moving in.
    /// </summary>
    private void Flip()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y += 180;

        //Invert the boolean.
        facingRight = !facingRight;

        transform.rotation = Quaternion.Euler(rot);

        //Changing umbrella direction
        if (umbrellaRight)
        {
            umbrellaRight = false;
            umbrellaLeft = true;
        }
        else if (umbrellaLeft)
        {
            umbrellaLeft = false;
            umbrellaRight = true;
        }
    }

    /// <summary>
    /// If the player presses the space bar, the umbrella will open or close,
    /// the swap bool will be swaped in the swap function which is invoked
    /// after the player presses spacebar. If the umbrella is left when the
    /// player opens the umbrella they will dash right.
    /// Connor Riley
    /// </summary>
    void ActivateUmbrella()
    {
        if (Input.GetKeyDown(KeyCode.Space) && umbrella == false)
        {
            //playerUmbrella.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            umbrella = true;
            

            if (umbrellaLeft && canDash)
            {
                rb.AddForce(new Vector2(1.0f, 0.0f) * dashForce, ForceMode2D.Impulse);
                StopCoroutine(ResetDash());
                StartCoroutine(ResetDash());
            }
            else if (umbrellaRight && canDash)
            {
                rb.AddForce(new Vector2(-1.0f, 0.0f) * dashForce, ForceMode2D.Impulse);
                StopCoroutine(ResetDash());
                StartCoroutine(ResetDash());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && umbrella == true)
        {
            //playerUmbrella.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            umbrella = false;
        }
        UpdateSprite();
    }

    private int dashNum = 0;

    IEnumerator ResetDash()
    {
        print("Starting: " + dashNum);
        dashNum++;

        if (dashNum > 1)
        {
            dashNum--;
            yield break;
        }

        yield return new WaitForSeconds(dashTime);
        canDash = false;
        yield return new WaitForSeconds(dashCooldownTime);
        canDash = true;
        print("finishing");
        dashNum--;
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
            umbrellaUp = true;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Up");
        }

        if (Input.GetButton("UmbrellaRight") && facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = true;
            ChangeUmbrellaSprite("Right");
        }

        if (Input.GetButton("UmbrellaRight") && !facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = true;
            ChangeUmbrellaSprite("Left");
        }

        if (Input.GetButton("UmbrellaDown"))
        {
            umbrellaUp = false;
            umbrellaDown = true;
            umbrellaLeft = false;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Down");
        }

        if (Input.GetButton("UmbrellaLeft") && facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = true;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Left");
        }

        if (Input.GetButton("UmbrellaLeft") && !facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = true;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Right");
        }
    }

    void HandleGrabbing(Rigidbody2D obj)
    {
        //If the player presses 'F' and the object is NOT already grabbed, freeze it and update its movement to move with the player.
        if (Input.GetKeyDown(KeyCode.F) && !objectGrabbed)
        {
            obj.constraints = RigidbodyConstraints2D.FreezeAll;
            objectGrabbed = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && objectGrabbed)
        {
            obj.constraints = RigidbodyConstraints2D.None;
            objectGrabbed = false;
        }
        else if (objectGrabbed)
        {
            obj.transform.position = objHoldPos.position;
        }
    }

    /// <summary>
    /// When the player collides with objects. In this case, spikes.
    /// </summary>
    /// <param name="collision">Object the player collided with.</param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // If the collided object is a spike trap, reduce player's lives
        //and update UI.
        if (collision.gameObject.tag == "Spikes")
        {
            //Will only be called if the player is invincible (check the RemoveLivesFromPlayer function in the GameControllerScript).
            GameControllerScript.instance.RemoveLivesFromPlayer(1);
        }
    }

    void ChangeUmbrellaSprite(string direction)
    {
        SpriteRenderer spriteDirection = umbrellaObject.GetComponent<SpriteRenderer>();
        int index = 0;
        string dir = direction;
        switch (dir)
        {
            case ("Right"): index = 0;
                break;
            case ("Up"): index = 1;
                break;
            case ("Left"): index = 2;
                break;
            case ("Down"): index = 3; 
                break;
        }
        spriteDirection.sprite = spriteArray[index];
        if (umbrella)
        {
            spriteDirection.sprite = spriteArray[index + 4];
        }
    }
    void UpdateSprite()
    {
        if (umbrellaRight) { ChangeUmbrellaSprite("Right"); }
        if (umbrellaUp) { ChangeUmbrellaSprite("Up"); }
        if (umbrellaLeft) { ChangeUmbrellaSprite("Left"); }
        if (umbrellaDown) { ChangeUmbrellaSprite("Down"); }
    }
}
