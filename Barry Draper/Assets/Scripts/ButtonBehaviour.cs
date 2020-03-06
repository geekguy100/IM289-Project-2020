/*****************************************************************************
// File Name : ButtonBehaviour
// Author : Kyle Grenier
// Creation Date : February 12, 2020
//
// Brief Description : Behaviour for interactable buttons around the game world. Buttons can turn on Interactables (fans, doors, etc.).
*****************************************************************************/

using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    [Header("Power System")]
    /// <summary>
    /// Does the button need a weighted object to be powered?
    /// </summary>
    public bool weighted = true;
    private bool isPowered = false; //Is the button being powered?
    public InteractableBehaviour[] interactables; //The interactable attached to this button.
    private int contacts; //The number of objects currently on the button.

    [Header("Colors")]
    public Color offColor; //color when the button is turned off.
    public Color onColor; //color when the button is turned on.

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = offColor;
    }

    private void OnTriggerEnter2D()
    {
        contacts++;
    }

    //Used for weighted buttons.
    private void OnTriggerStay2D(Collider2D col)
    {
        if (weighted && !isPowered)
        {
            PowerOnButton();
        }
    }

    //Powers off the button when an object leaves it.
    private void OnTriggerExit2D(Collider2D col)
    {
        contacts--;
        //If the button is weighted and has no more contacts on it.
        if (weighted && contacts < 1)
            PowerOffButton();
    }




    private void PowerOnButton()
    {
        isPowered = true;
        sr.color = onColor;
        //TODO: Play sound.

        foreach (InteractableBehaviour interactable in interactables)
        {
            //Power on the attached interactable if it is not already.
            if (interactable != null && !interactable.IsPowered())
            {
                interactable.PowerOn();
            }
            else if (interactable == null)
            {
                Debug.LogWarning("There is no Interactable attached to this button!: " + gameObject.name);
            }
        }
    }

    private void PowerOffButton()
    {
        isPowered = false;
        sr.color = offColor;
        //TODO: Play sound.

        foreach (InteractableBehaviour interactable in interactables)
        {
            //Power on the attached interactable if it is not already.
            if (interactable != null && interactable.IsPowered())
            {
                interactable.PowerOff();
            }
            else if (interactable == null)
            {
                Debug.LogWarning("There is no Interactable attached to this button!: " + gameObject.name);
            }
        }
    }
}
