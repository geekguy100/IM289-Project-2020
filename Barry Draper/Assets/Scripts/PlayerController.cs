/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Kyle Grenier, Connor Riley
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
    [HideInInspector]
    public Vector2 newPos;
    public float minYVel = -4.9f;

    [Header("Dashing")]
    public float dashForce = 5.0f;
    public float maxXVel = 15f;

    [Header("Movement Dependencies")]
    public Transform groundPosition;
    public LayerMask whatIsGround;
    private bool isGrounded = false;
    private bool facingRight = true;
    public float fallDamageVelocity = -10f;
    public float fallDamageAmnt = 1f;
    private bool willTakeFallDamage = false;

    [Header("Game Objects")]                                /*CD*/
    [Tooltip("The sprite for the player's umbrella.")]      /*CD*/
    public GameObject umbrellaObject;


    //Is the players umbrella is activated or not
    [HideInInspector]
    public bool umbrella = false;

    [Header("Rotations of the umbrella")]
    Quaternion up = Quaternion.Euler(new Vector3(0, 0, 0));
    Quaternion right = Quaternion.Euler(new Vector3(0, 0, 270));
    Quaternion down = Quaternion.Euler(new Vector3(0, 0, 180));
    Quaternion left = Quaternion.Euler(new Vector3(0, 0, 90));

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
    private Rigidbody2D theRigidbodyInRange;
    private GameObject objectInRange;

    [Header("Umbrella Shield Properties")]
    public BoxCollider2D umbrellaShieldTrigger;
    public Vector2 shieldOffsetHigh;
    public Vector2 shieldOffsetLow;

    //The offset of where the ray will be casted in relation to the player's position.
    public Vector2 grabOffset = Vector3.zero;
    private bool objectGrabbed = false;

    [Header("Animation Attributes")]
    private Animator anim;
    public Sprite[] spriteArray = new Sprite[8];
    /* 0 = Col Right    |   4 = Act Right
     * 1 = Col Up       |   5 = Act Up
     * 2 = Col Left     |   6 = Act Left
     * 3 = Col Down     |   7 = Act Down
    */

    GameObject umbrellaSprite;
    private int currentIndex;

    //Sprite renderer for upperbody. Used to make the player flash upon taking damage.
    private SpriteRenderer sr;

    //Audio and SFX
    private AudioController audioController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioController = GetComponentInChildren<AudioController>();
        sr = transform.GetChild(3).GetComponent<SpriteRenderer>();

        //Initial values for player direction.
        umbrellaShieldTrigger.enabled = false;
        umbrellaUp = false;
        umbrellaDown = false;
        umbrellaLeft = false;
        umbrellaRight = true;
        ChangeUmbrellaSprite("Right");
        umbrellaShieldTrigger.offset = shieldOffsetLow;

        deathAnim.SetActive(false);
    }

    //Used to visualize the box cast used to check if the player is grounded. KG
    float boxCastYScale = 0.05f;
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the groundPosition's position. KG
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(groundPosition.position, new Vector2(transform.localScale.x, boxCastYScale));
    }

    private bool deathAnimRan = false;

    void Update()
    {
        //If the player is in free camera mode, don't run because the player should be unable to move.
        if (GameControllerScript.instance.GetFreeCamMode())
        {
            anim.SetBool("isGrounded", false);
            audioController.StopPlayingLoop();
            return;
        }

        //If the player is not alive don't bother running the code in this function.
        if (!GameControllerScript.instance.playerAlive && !deathAnimRan)
        {
            death();
            return;
        }
        else if (!GameControllerScript.instance.playerAlive)
        {
            return;
        }

        //Updating the newPos x value (where the player will move to)
        //based on if the player is grounded or not. KG
        float xMov = Input.GetAxis("Horizontal");

        //Updating player's direction (which direction he is moving).
        if ((xMov > 0 && !facingRight) || (xMov < 0 && facingRight))
        {
            Flip();
        }

        //Checking if the player is grounded.
        isGrounded = Physics2D.BoxCast(groundPosition.position, new Vector2(Mathf.Abs(transform.localScale.x), boxCastYScale), 0f, Vector3.zero, 0f, whatIsGround);

        //Updating the x value accordingly.
        if (isGrounded && rb.velocity.y == 0)
        {
            newPos.x = xMov * moveSpeed;
            if (willTakeFallDamage)
            {
                print("You took fall damage!");
                GameControllerScript.instance.RemoveLivesFromPlayer(fallDamageAmnt);
                willTakeFallDamage = false;
            }
        }
        else
        {
            newPos.x = xMov * airSpeed;

            if (!(umbrella && umbrellaUp))
            {
                if (!willTakeFallDamage && rb.velocity.y <= fallDamageVelocity)
                {
                    print("Ur gonna take fall damage!");
                    willTakeFallDamage = true;
                }
                else if (willTakeFallDamage && rb.velocity.y > fallDamageVelocity)
                {
                    print("No more fall damage!");
                    willTakeFallDamage = false;
                }
            }
            else if (willTakeFallDamage)
            {
                print("You saved yourself from fall damage");
                willTakeFallDamage = false;
            }
        }
            

        anim.SetFloat("xMov", xMov);
        anim.SetBool("isGrounded", isGrounded);

        //If player is moving on the ground, play the run sfx.
        if (xMov != 0 && isGrounded)
            audioController.PlayClip(AudioController.PlayerSFX.playerWalk, true, true);
        else
            audioController.StopPlayingLoop();

        ActivateUmbrella();
        PointUmbrella();

        if (objectInRange || objectGrabbed)
        {
            if (objectGrabbed)
            {
                HandleGrabbing(theRigidbodyInRange);
            }
            else if (!objectGrabbed)
            {
                theRigidbodyInRange = objectInRange.transform.GetComponent<Rigidbody2D>();
                HandleGrabbing(theRigidbodyInRange);
            }
        }
    }

    private float minYVelWithUmbrella = -4.9f;
    private float minYVelWithoutUmbrella = -15f;

    private void FixedUpdate()
    {
        //If the player is not alive don't bother running the code in this function.
        //If the player is in free camera mode, don't let them move -- the WASD keys are used for moving the camera!
        if (!GameControllerScript.instance.playerAlive || GameControllerScript.instance.GetFreeCamMode())
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

        //If the x velocity's direction and desired position conflict, set the velocity to 0.
        if ((rb.velocity.x < 0 && newPos.x > 0) || (rb.velocity.x > 0 && newPos.x < 0))
        {
           // print("Conflict between velocity.x and newPos.x");
            Vector2 vel = rb.velocity;
            vel.x = 0;
            rb.velocity = vel;
        }

        //Clamping the x velocity of the player so they cannot dash infinitely fast.
        Vector2 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(rb.velocity.x, -maxXVel, maxXVel);
        rb.velocity = velocity;

        rb.position = pos;
    }

    private void UpdateObjectHoldPosition(bool toTheRight)
    {
        Vector3 pos = objHoldPos.localPosition;
        Vector3 pos2 = rayStart.localPosition;

        if (toTheRight)
        {
            if (pos.x < 0)
                pos.x *= -1;
            if (pos2.x < 0)
            {
                pos2.x *= -1;
                grabDistance *= -1;
            }
        }
        else
        {
            if (pos.x > 0)
                pos.x *= -1;
            if (pos2.x > 0)
            {
                pos2.x *= -1;
                grabDistance *= -1;
            }
        }

        objHoldPos.localPosition = pos;
        rayStart.localPosition = pos2;
    }

    /// <summary>
    /// Flip the player transform horizontally so they are facing in the direction they are moving in.
    /// </summary>
    private void Flip()
    {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = !facingRight; //Puts the legs in the right direction.

        SpriteRenderer bustSpriteRenderer = umbrellaObject.GetComponent<SpriteRenderer>();

        if (facingRight && umbrellaRight)
        {
            ChangeUmbrellaSprite("Right");
            bustSpriteRenderer.flipX = false;
            UpdateObjectHoldPosition(true);

            umbrellaShieldTrigger.offset = shieldOffsetLow;
        }
        else if (!facingRight && umbrellaRight)
        {
            ChangeUmbrellaSprite("Left");
            bustSpriteRenderer.flipX = true;
            UpdateObjectHoldPosition(false);

            umbrellaShieldTrigger.offset = shieldOffsetHigh;
        }
        else if (facingRight && umbrellaLeft)
        {
            ChangeUmbrellaSprite("Left");
            bustSpriteRenderer.flipX = false;
            UpdateObjectHoldPosition(true);

            Vector2 shieldOffset = shieldOffsetHigh;
            shieldOffset.x = -shieldOffset.x;
            umbrellaShieldTrigger.offset = shieldOffset;
        }
        else if (!facingRight && umbrellaLeft)
        {
            ChangeUmbrellaSprite("Right");
            bustSpriteRenderer.flipX = true;
            UpdateObjectHoldPosition(false);

            Vector2 shieldOffset = shieldOffsetLow;
            shieldOffset.x = -shieldOffset.x;
            umbrellaShieldTrigger.offset = shieldOffset;
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
        //Don't allow the player to use the umbrella while in free cam mode.
        if (GameControllerScript.instance.GetFreeCamMode())
            return;

        if (Input.GetButtonDown("Jump") && umbrella == false)
        {
            umbrella = true;
            OpenUmbrellaSprite();
            if (!umbrellaUp && !umbrellaDown) //Make sure this doesn't enable when the player has umbrella up or down.
                umbrellaShieldTrigger.enabled = true;
            audioController.PlayClip(AudioController.PlayerSFX.openUmbrella);

            if (umbrellaLeft)
            {
                rb.AddForce(new Vector2(1.0f, 0.0f) * dashForce, ForceMode2D.Impulse);
                //StopCoroutine(ResetDash());
                //StartCoroutine(ResetDash());
            }
            else if (umbrellaRight)
            {
                rb.AddForce(new Vector2(-1.0f, 0.0f) * dashForce, ForceMode2D.Impulse);
                //StopCoroutine(ResetDash());
                //StartCoroutine(ResetDash());
            }
        }
        else if (Input.GetButtonDown("Jump") && umbrella == true)
        {
            umbrella = false;
            OpenUmbrellaSprite();
            umbrellaShieldTrigger.enabled = false;
            audioController.PlayClip(AudioController.PlayerSFX.closeUmbrella);
        }
    }


    /// <summary>
    /// Switches the rotation of the players umbrella by pressing the
    /// up, down, left, and right arrow keys.
    /// Connor Riley
    /// </summary>
    void PointUmbrella()
    {
        //Don't allow the player to point the umbrella while in free cam mode.
        if (GameControllerScript.instance.GetFreeCamMode())
            return;

        SpriteRenderer bustSpriteRenderer = umbrellaObject.GetComponent<SpriteRenderer>();

        if (Input.GetButtonDown("UmbrellaRight") && facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = true;
            ChangeUmbrellaSprite("Right");
            bustSpriteRenderer.flipX = false;

            umbrellaShieldTrigger.offset = shieldOffsetLow;

            if (umbrella)
            {
                umbrellaShieldTrigger.enabled = true;

            }
        }
        else if (Input.GetButtonDown("UmbrellaRight") && !facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = true;
            ChangeUmbrellaSprite("Left");
            bustSpriteRenderer.flipX = true;

            umbrellaShieldTrigger.offset = shieldOffsetHigh;

            if (umbrella)
            {
                umbrellaShieldTrigger.enabled = true;
            }
        }
        else if (Input.GetButtonDown("UmbrellaLeft") && facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = true;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Left");
            bustSpriteRenderer.flipX = false;

            Vector2 shieldOffset = shieldOffsetHigh;
            shieldOffset.x = -shieldOffset.x;
            umbrellaShieldTrigger.offset = shieldOffset;

            if (umbrella)
            {
                umbrellaShieldTrigger.enabled = true;
            }
        }
        else if (Input.GetButtonDown("UmbrellaLeft") && !facingRight)
        {
            umbrellaUp = false;
            umbrellaDown = false;
            umbrellaLeft = true;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Right");
            bustSpriteRenderer.flipX = true;

            Vector2 shieldOffset = shieldOffsetLow;
            shieldOffset.x = -shieldOffset.x;
            umbrellaShieldTrigger.offset = shieldOffset;

            if (umbrella)
            {
                umbrellaShieldTrigger.enabled = true;
            }
        }
        else if (Input.GetButtonDown("UmbrellaUp"))
        {
            umbrellaUp = true;
            umbrellaDown = false;
            umbrellaLeft = false;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Up");
            bustSpriteRenderer.flipX = false;

            umbrellaShieldTrigger.enabled = false;
        }
        else if (Input.GetButtonDown("UmbrellaDown"))
        {
            umbrellaUp = false;
            umbrellaDown = true;
            umbrellaLeft = false;
            umbrellaRight = false;
            ChangeUmbrellaSprite("Down");
            bustSpriteRenderer.flipX = false;

            umbrellaShieldTrigger.enabled = false;
        }
    }

    private NpcInteraction NPC;

    private void OnTriggerStay2D(Collider2D col)
    {
        //If the player is within the range of an NPC and wants to interact with them.
        if (col.gameObject.CompareTag("NPC") && !NPC)
        {
            NPC = col.gameObject.GetComponent<NpcInteraction>();
            NPC.HandleInteraction();
        }
        //If the gameobject is a box, be able to pick it up.
        else if (col.gameObject.CompareTag("Grabbable") && !objectGrabbed)
        {
            //print("Object in range! " + col.transform.parent.name);
            objectInRange = col.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //If the gameobject is an interactable, don't be able to pick it up.
        if (col.gameObject.CompareTag("Grabbable"))
            objectInRange = null;
        else if (col.gameObject.CompareTag("NPC"))
        {
            NPC.StopInteraction();
            NPC = null;
        }
    }

    void HandleGrabbing(Rigidbody2D obj)
    {
        //If the player presses 'F' and the object is NOT already grabbed, freeze it and update its movement to move with the player.
        if (Input.GetButtonDown("Grab Object") && !objectGrabbed)
        {
            obj.constraints = RigidbodyConstraints2D.FreezeAll;
            objectGrabbed = true;

            audioController.PlayClip(AudioController.PlayerSFX.pickupBox);
            obj.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (Input.GetButtonDown("Grab Object") && objectGrabbed)
        {
            obj.constraints = RigidbodyConstraints2D.None;
            audioController.PlayClip(AudioController.PlayerSFX.dropBox);
            obj.GetComponent<BoxCollider2D>().enabled = true;
            objectGrabbed = false;
            objectInRange = null;
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
            case ("Right"):
                index = 0;
                break;
            case ("Up"):
                index = 1;
                break;
            case ("Left"):
                index = 2;
                break;
            case ("Down"):
                index = 3;
                break;
        }
        spriteDirection.sprite = spriteArray[index];

        if (umbrella)
        {
            index += 4;
            spriteDirection.sprite = spriteArray[index];
        }


        currentIndex = index;
    }

    void OpenUmbrellaSprite()
    {
        SpriteRenderer spriteDirection = umbrellaObject.GetComponent<SpriteRenderer>();
        if (umbrella)
        {
            currentIndex += 4;
        }
        else if (!umbrella)
        {
            currentIndex -= 4;
        }

        spriteDirection.sprite = spriteArray[currentIndex];
    }

    public GameObject deathAnim;

    public void StartFlash()
    {
        StartCoroutine("Flash");
    }

    private IEnumerator Flash()
    {
        float t = 0.05f;
        while (true)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(t);
            sr.color = Color.white;
            yield return new WaitForSeconds(t);
        }
    }

    public void StopFlash()
    {
        StopCoroutine("Flash");
        sr.color = Color.white;
    }

    void death()
    {
        GameControllerScript.instance.playerAlive = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        deathAnim.SetActive(true);
        umbrellaObject.SetActive(false);

        deathAnimRan = true;
    }
}