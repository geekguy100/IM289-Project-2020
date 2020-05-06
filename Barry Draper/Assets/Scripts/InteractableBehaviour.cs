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
    [Header("Fan Movment System")]
    //Should the fan be a moving fan?
    public bool fanMoves = false;
    public Vector3 startPos;
    public Vector3 endPos;
    public float fanMoveSpeed = 1f;
    private bool fanMoving = false;
    private Vector3 currentDestination;

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

    private AudioController audioController;

    private Animator anim;

    private Coroutine lastRoutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioController = GetComponentInChildren<AudioController>();

        //Animation only relevent to the fans
        if (type == InteractableType.Fan)
            anim = GetComponent<Animator>();
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
            if (fanMoves)
                currentDestination = endPos;

            if (isPowered)
                PowerOn();
            else
                PowerOff();
        }


        sr.color = offColor;

        //InvokeRepeating("Flip", 0.1f, time);

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
        }

        audioController.PlayClip(AudioController.FanSFX.fanWhir, true);
        anim.SetBool("Powered", true);

        //If the fan is a moving fan, make it move!
        if (fanMoves)
            lastRoutine = StartCoroutine(MoveFan());
    }

    /// <summary>
    /// Contains code that controls what the fan does when powered off.
    /// </summary>
    private void FanOffAction()
    {
        foreach(GameObject draft in drafts)
        {
            draft.SetActive(false);
        }

        audioController.StopPlayingLoop();
        anim.SetBool("Powered", false);

        //If the fan is a moving fan, make sure to make it stop moving!
        if (fanMoves)
        {
            print("Stopping fan!");

            if (lastRoutine != null)
                StopCoroutine(lastRoutine);

            fanMoving = false;
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
    //private void move(bool moveX, bool moveY, int speed)
    //{
    //    if(moveX && moveY)
    //    {
    //        transform.Translate(diagonal * speed * Time.deltaTime);
    //    }
    //    else
    //    if(moveX)
    //    {
    //        //If the fan hasn't reached its target, keep moving it.
    //        if (Mathf.Abs(distance - currentDistance) != 0)
    //            transform.position += Vector3.right * speed * Time.deltaTime;
    //        else
    //            Flip();
    //    }
    //    else
    //    if(moveY)
    //    {
    //        transform.Translate(Vector2.up * speed * Time.deltaTime);
    //    }
    //}

    private IEnumerator MoveFan()
    {
        Vector2 cPos = transform.position;
        fanMoving = true;
        while (fanMoving)
        {
            Vector2 dir = (transform.position - currentDestination).normalized;
            Vector2 vel = dir * fanMoveSpeed;

            while (Vector2.Distance(cPos, currentDestination) > 0.05f)
            {
                cPos -= vel * Time.deltaTime;
                transform.position = cPos;

                //If the fan should NOT be moving mid transformation, just break out.
                //if (!fanMoving)
                //    yield break;

                yield return null;
            }

            if (currentDestination == endPos)
                currentDestination = startPos;
            else
                currentDestination = endPos;

            yield return null;
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
        fanMoveSpeed *= -1;
    }
}