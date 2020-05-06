/*****************************************************************************
// File Name : BulletBehaviour.cs
// Author : Connor Dunn
// Creation Date : February 19, 2020
//
// Brief Description : Bullets fly forward and impact the player and turrets upon reflection from shield.
*****************************************************************************/

using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Header("Main Attributes")]
    public float bulletSpeed = 5;
    public float destroyTime = 10f;

    [Header("Damage Attributes")]
    public int damageDealt = 1;

    private Rigidbody2D rb;
    private Vector3 direction;

    private bool canHurtMinion = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * Time.fixedDeltaTime * bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Umbrella"))
        {
            direction = -direction;

            //Once the bullet bounces off of the umbrella, it can hurt minions.
            //This is to prevent minions from firing at each other.
            canHurtMinion = true;
        }
        else if (col.gameObject.CompareTag("Player"))
        {
            //Note: Player hit SFX is handled in the GameControllerScript.

            //Only interact with the BoxCollider2D of the player; the CircleCollider2D trigger is used for
            //detecting when objects are in range.
            if (col.GetType() == typeof (BoxCollider2D))
            {
                GameControllerScript.instance.RemoveLivesFromPlayer(1);
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.CompareTag("Minion"))
        {
            //Play minion hurt SFX.
            if (canHurtMinion)
            {
                col.gameObject.GetComponent<MinionHealthBehaviour>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.CompareTag("Turret"))
        {
            col.gameObject.GetComponent<TurretHealthBehaviour>().TakeDamage(damageDealt);
            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}