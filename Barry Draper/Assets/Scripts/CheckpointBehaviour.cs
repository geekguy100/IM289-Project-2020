/*****************************************************************************
// File Name : CheckpointBehaviour
// Author : Kyle Grenier
// Creation Date : April 20, 2020
//
// Brief Description : Controls behaviour for when the player passes through a checkpoint
*****************************************************************************/

using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    private bool triggered = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Make sure not to run for the player's circle collider.
            if (col.GetType() == typeof(CircleCollider2D))
                return;
            else if (!triggered)
            {
                triggered = true;
                anim.SetBool("flip", true);
                GameControllerScript.instance.UpdateCheckpointPos(transform.position);
            }
        }
    }
}
