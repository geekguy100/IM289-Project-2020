using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame
    void Update()
    {
        
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
