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
    public InteractableBehaviour interactable; //The interactable attached to this button.

    [Header("Colors")]
    public Color offColor; //color when the button is turned off.
    public Color onColor; //color when the button is turned on.

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = offColor;
    }

    //Used for weighted buttons.
    private void OnTriggerStay2D(Collider2D col)
    {
        if (weighted && !isPowered)
        {
            PowerOnButton();
        }
    }

    //Used for weighting buttons.
    private void OnTriggerExit2D(Collider2D col)
    {
        PowerOffButton();
    }




    private void PowerOnButton()
    {
        isPowered = true;
        sr.color = onColor;
        //TODO: Play sound.

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

    private void PowerOffButton()
    {
        isPowered = false;
        sr.color = offColor;
        //TODO: Play sound.

        //Power off the attached interactable if it is not already.
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