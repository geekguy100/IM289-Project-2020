/*****************************************************************************
// File Name : InteractableBehaviour
// Author : Kyle Grenier (100%)
                Implemented full functionality. (2/29/2020)
// Creation Date : February 12, 2020
//
// Brief Description : Script to control behaviour of interactables such as fans, doors, etc.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehaviour : MonoBehaviour
{
    public enum InteractableType { Fan, Door };
    private SpriteRenderer sr;

    //CR
    [Header("Movment System")]
    public bool stayOn = false;
    public bool moving = false;
    public bool yMove = false;
    public bool xMove = false;
    public int speed = 0;
    public float time = 0;
    private Vector3 diagonal = new Vector3(2.5f, 0f, 2.5f);

    [Header("Power System")]
    public InteractableType type;
    public bool isPowered = false;

    [Header("Colors")]
    public Color onColor;
    public Color offColor;

    [Header("Fan Attributes")]
    public GameObject[] drafts;

    [Header("Door Attributes")]
    public float openSpeed = 1f;
    public float closeSpeed = 1f;
    /// <summary>
    /// The number of units in the Y-axis to move.
    /// </summary>
    [Tooltip("The number of units in the Y-axis to move.")]
    public float openDistance = 5f;
    private Vector2 closedPos = Vector2.zero;
    private Vector2 openedPos = Vector2.zero;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (type == InteractableType.Door)
        {
            openedPos = transform.position;
            openedPos.y += openDistance;

            closedPos = transform.position;

            if (isPowered)
            {
                //StartCoroutine(DoorAction());
                PowerOn();
            }
        }
        else if (type == InteractableType.Fan)
        {
            if (isPowered)
                PowerOn();
            else
                PowerOff();
        }


        sr.color = offColor;

        InvokeRepeating("Flip", 0.1f, time);

    }

    private void Update()
    {   
        if (moving && isPowered) //CR
        {
            move(xMove, yMove, speed);
        }
    }



    //Power on the interactable and peform appropriate functions.
    public void PowerOn()
    {
        isPowered = true;
        sr.color = onColor;

        switch (type)
        {
            case InteractableType.Fan:
                FanAction();
                break;
            case InteractableType.Door:
                StopCoroutine("DoorOffAction");
                StartCoroutine("DoorAction");
                break;
        }
    }

    //Power off the interactable and peform appropriate functions.
    public void PowerOff()
    {
        isPowered = false;
        sr.color = offColor;

        switch (type)
        {
            case InteractableType.Fan:
                FanOffAction();
                break;
            case InteractableType.Door:
                StopCoroutine("DoorAction");
                StartCoroutine("DoorOffAction");
                break;
        }
    }

    /// <summary>
    /// Is the interactable receiving power?
    /// </summary>
    /// <returns>True if the interactable is receiving power.</returns>
    public bool IsPowered()
    {
        return isPowered;
    }



    /// <summary>
    /// Contains code that controls what the fan does when powered on.
    /// </summary>
    private void FanAction()
    {
        foreach(GameObject draft in drafts)
        {
            draft.SetActive(true);
            moving = true;
        }
    }

    /// <summary>
    /// Contains code that controls what the fan does when powered off.
    /// </summary>
    private void FanOffAction()
    {
        foreach(GameObject draft in drafts)
        {
            if(!stayOn)
            {
                draft.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Contains code that controls what the door does when powered on.
    /// </summary>
    private IEnumerator DoorAction()
    {
        Vector2 cPos = transform.position;
        
        while(Vector2.Distance(cPos, openedPos) > 0.05f)
        {
            cPos.y += openSpeed * Time.deltaTime;
            transform.position = cPos;
            yield return null;
        } 
    }

    /// <summary>
    /// Contains code that controls what the door does when powered off.
    /// </summary>
    private IEnumerator DoorOffAction()
    {
        Vector2 cPos = transform.position;

        while (Vector2.Distance(cPos, closedPos) > 0.05f)
        {
            cPos.y -=  closeSpeed * Time.deltaTime;
            transform.position = cPos;
            yield return null;
        }
    }

    /// <summary>
    /// Handles the movement of fans. Takes in if the
    /// fan is moving on the X or Y axis, and then 
    /// moves it accordingly to the speed. 
    /// CR
    /// </summary>
    private void move(bool moveX, bool moveY, int speed)
    {
        if(moveX && moveY)
        {
            transform.Translate(diagonal * speed * Time.deltaTime);
        }

        if(moveX)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        if(moveY)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }


    /// <summary>
    /// Multiplies speed by -1 so that the fan will
    /// start moving in the opposite direction of the 
    /// direction that it is currently moving in. CR
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    private void Flip()
    {
        speed *= -1;
    }
}