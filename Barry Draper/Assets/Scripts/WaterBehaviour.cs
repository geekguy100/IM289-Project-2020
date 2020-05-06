/*****************************************************************************
// File Name :         DraftBehaviour.cs
// Author :            Connor Riley
// Creation Date :     March 6, 2020
//
// Brief Description : Controls the behaviour of water in the game.
                       Determines how the player interacts with that water.
*****************************************************************************/
using UnityEngine;

public class WaterBehaviour : MonoBehaviour
{
    public int waterWalkSpeed = 2;
    public int hoverSpeed = 20;

    public Transform maxHeight;

    public GameObject waterParticle;
    private GameObject instantiation = null;

    private float previousMoveSpeed;
    private float previousAirSpeed;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        //If the player didn't enter don't bother running the code.
        if (!col.CompareTag("Player"))
            return;
        else if (col.GetType() == typeof(CircleCollider2D))
            return;

        PlayerController pc = col.gameObject.GetComponent<PlayerController>();
        Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();

        //Assign previous speeds and input the new speeds.
        previousMoveSpeed = pc.moveSpeed;
        previousAirSpeed = pc.airSpeed;
        pc.moveSpeed = waterWalkSpeed;

        //Divide the player's velocity in half so he won't take fall damage when hitting the water IF they're moving too fast.
        if (rb2d.velocity.y < -12f)
            rb2d.velocity = rb2d.velocity / 2;

        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Play the splash SFX if it isn't already playing.
        if(!audioSource.isPlaying)
            audioSource.Play();
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;
        //Ignore the player's circle collider cause that's used for checking if boxes are nearby.
        else if (col.GetType() == typeof(CircleCollider2D))
            return;


        PlayerController pc = col.gameObject.GetComponent<PlayerController>();
        DrowningBehaviour playerDrowning = col.gameObject.GetComponent<DrowningBehaviour>();
        Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();

        //If the player is below the maxHeight's y position, he will drown and take damage.
        playerDrowning.CheckHeight(maxHeight);

        if (pc.umbrellaDown && pc.umbrella)
        {
            rb2d.AddForce(Vector2.up * hoverSpeed, ForceMode2D.Force);

            if (instantiation == null)
                CreateParticle(col.transform);
        }
        else
        {
            if (instantiation != null)
                Destroy(instantiation);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;
        //Ignore the player's circle collider cause that's used for checking if boxes are nearby.
        else if (col.GetType() == typeof(CircleCollider2D))
            return;


        Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();
        PlayerController pc = col.gameObject.GetComponent<PlayerController>();
        DrowningBehaviour db = col.gameObject.GetComponent<DrowningBehaviour>();

        //rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        pc.moveSpeed = previousMoveSpeed;
        pc.airSpeed = previousAirSpeed;
        Destroy(instantiation);

        db.drownbar.gameObject.SetActive(false);
        db.health = db.breathTime;
        db.drownbar.maxValue = db.breathTime;
        db.drownbar.value = db.health;
    }


    void CreateParticle(Transform parent)
    {
        instantiation = Instantiate(waterParticle, new Vector3(parent.position.x, parent.position.y - 2f, parent.position.z), Quaternion.identity,parent);
        instantiation.transform.localScale = new Vector3(1f, 1f, 1f);
        instantiation.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
    }
}
