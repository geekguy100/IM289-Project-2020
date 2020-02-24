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
        }


        sr.color = offColor;
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
        print("Fan is on!!");
        foreach(GameObject draft in drafts)
        {
            draft.SetActive(true);
        }
    }

    /// <summary>
    /// Contains code that controls what the fan does when powered off.
    /// </summary>
    private void FanOffAction()
    {
        print("Fan just turned off.");
        foreach(GameObject draft in drafts)
        {
            draft.SetActive(false);
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
}