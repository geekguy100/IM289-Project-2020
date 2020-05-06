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
    private AudioSource audioSource;

    private void Awake()
    {
        //Get the Animator component in the child (paper).
        anim = transform.GetChild(0).GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    //Handle the interation with the player.
    public void HandleInteraction()
    {
        //If the dialogue is already on screen, return.
        if (interacting)
            return;

        interacting = true;
        anim.SetBool("Interacting", interacting);

        //Due to how I designed the player's ability to pick up boxes and move through them, this HandleInteraction function will get called twice if the player
        //is within range of the npc AND inside of the npc. To counter that, we make sure that the audio source is NOT playing AND that the animation state 
        //is NOT already in the popUp animation.
        if (!audioSource.isPlaying && anim.GetCurrentAnimatorStateInfo(0).fullPathHash != -1029435967)
            audioSource.Play();
    }

    //Set interacting to false and make the paper pop back down.
    public void StopInteraction()
    {
        interacting = false;
        anim.SetBool("Interacting", interacting);
    }
}