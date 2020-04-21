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

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            //Play pop up animation.
            anim.SetTrigger("Pop Up");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (col.GetType() == typeof(BoxCollider2D))
                return;

            //Play pop down animaiton.
            anim.SetTrigger("Pop Down");
        }
    }
}