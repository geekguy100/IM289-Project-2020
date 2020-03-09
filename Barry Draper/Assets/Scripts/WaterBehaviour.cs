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
    int waterSpeed = 10;

    int hoverSpeed = 150;

    public GameObject waterParticle;

    GameObject newWater;

    bool created = false;

    private void Start()
    {
        newWater = waterParticle;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController pc = col.gameObject.GetComponent
                                       <PlayerController>();

        Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();

        if (newWater && newWater.activeSelf == false && pc && pc.umbrellaDown)
        {
            newWater.SetActive(true);
        }

        else if (col.gameObject.CompareTag("Player"))
        {

            if (pc && pc.umbrellaDown && rb2d && pc.umbrella)
            {
                 
                rb2d.AddForce(Vector2.up * hoverSpeed, ForceMode2D.Force);

                rb2d.constraints = RigidbodyConstraints2D.FreezePositionY |
                                      RigidbodyConstraints2D.FreezeRotation;



                if (created == false)
                {
                    newWater = Instantiate(waterParticle, new Vector3
                    (transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity);

                    newWater.transform.parent = col.gameObject.transform;
                    newWater.transform.position = new Vector3(2f, -3f, 0f);
                    newWater.transform.localScale = new Vector3(1f, 1f, 1f);
                    newWater.transform.rotation = 
                        Quaternion.Euler(-90f, 0f, 0f);
                }
                created = true;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player"))
        {

            PlayerController pc = col.gameObject.GetComponent
                                        <PlayerController>();

            Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();

            if(!pc.umbrellaDown)
            {
                isTrigger();
                pc.moveSpeed = 2;
                rb2d.constraints = 0;
                rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            if(pc.umbrellaDown)
            {
                rb2d.AddForce(Vector2.up * waterSpeed, ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Rigidbody2D rb2d = col.gameObject.GetComponent<Rigidbody2D>();

        if(rb2d)
        {
            rb2d.constraints = 0;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            Invoke("removeWater", 1.5f);
        }
    }

    void isTrigger()
    {
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    void isNotTrigger()
    {
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
    }

    void removeWater()
    {
        newWater.SetActive(false);
    }
}
