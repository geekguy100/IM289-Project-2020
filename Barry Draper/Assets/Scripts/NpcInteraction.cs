/*****************************************************************************
// File Name : NpcInteraction
// Author : Kyle Grenier
// Creation Date : May 05, 2020
//
// Brief Description : Behvaiour for interacting with the ghost NPC.
*****************************************************************************/

using UnityEngine;

public class NpcInteraction : MonoBehaviour
{
    private bool interacting = false;
    private Animator anim;

    private void Awake()
    {
        //Get the Animator component in the child (paper).
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    //Handle the interation with the player.
    public void HandleInteraction()
    {
        //If the dialogue is already on screen, return.
        if (interacting)
            return;

        interacting = true;
        anim.SetBool("Interacting", interacting);
    }

    //Set interacting to false and make the paper pop back down.
    public void StopInteraction()
    {
        interacting = false;
        anim.SetBool("Interacting", interacting);
    }
}