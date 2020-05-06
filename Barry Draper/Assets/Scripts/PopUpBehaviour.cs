/*****************************************************************************
// File Name : PopUpBehaviour
// Author : Kyle Grenier
// Creation Date : April 21, 2020
//
// Brief Description : Pops up a "tutorial" when the player is in range of a grabbable object.
*****************************************************************************/

using UnityEngine;

public class PopUpBehaviour : MonoBehaviour
{
    private Animator anim;
    private bool poppedUp = false;

    private void Awake()
    {
        anim = transform.parent.GetChild(0).GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //If the box is already popped up, don't bother trying to put it up a second time.
        if (poppedUp)
            return;

        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            print("Popping up because the player is nearby!");
            //Play pop up animation.
            poppedUp = true;
            anim.SetTrigger("Pop Up");
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        //If the player is nearby and this pop up is up and attached to a box, make the pop up go down if the player picks it up.
        if (poppedUp && col.gameObject.CompareTag("Player") && gameObject.CompareTag("Grabbable"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            //Check if the box is picked up (meaning its constraints will be FreezeAll).
            //If it is picked up, put the pop up down.
            Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
            if (rb.constraints.Equals(RigidbodyConstraints2D.FreezeAll))
            {
                print("Popping down because I'm picked up!");
                poppedUp = false;
                anim.SetTrigger("Pop Down");
            }
        }
        //If the same conditions are met, but the pop up is NOT up, meaning the player just dropped it, make sure to put the pop up back up.
        else if (!poppedUp && col.gameObject.CompareTag("Player") && gameObject.CompareTag("Grabbable"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
            if (rb.constraints.Equals(RigidbodyConstraints2D.None))
            {
                print("Popping up because I just got dropped!");
                poppedUp = true;
                anim.SetTrigger("Pop Up");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //If the pop up is NOT up, don't bother trying to put it down.
        if (!poppedUp)
            return;

        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            print("Popping down because the player left the trigger.");
            //Play pop down animaiton.
            poppedUp = false;
            anim.SetTrigger("Pop Down");
        }
    }
}