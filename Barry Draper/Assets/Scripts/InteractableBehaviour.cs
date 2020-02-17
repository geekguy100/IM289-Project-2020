/*****************************************************************************
// File Name : InteractableBehaviour
// Author : Kyle Grenier
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

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sr.color = offColor;
        if (!isPowered)
            PowerOff();
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
                DoorAction();
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
                DoorOffAction();
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
    private void DoorAction()
    {
        print("Door has opened");
    }

    private void DoorOffAction()
    {

    }

}
