/*****************************************************************************
// File Name : tutorialBehaviour
// Author : Connor Riley (100%)
                Implemented full functionality. (2/29/2020)
// Creation Date : February 12, 2020
//
// Brief Description : Script to control behaviour of the pop up tutorial
*****************************************************************************/
using UnityEngine;

public class tutorialBehaviour : MonoBehaviour
{
    public bool enter = false;

    private Animator anim; 

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            anim.SetBool("enter", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Grab Object"))
        {
            anim.SetBool("interact", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("enter", false);
        anim.SetBool("interact", false);
    }
}
