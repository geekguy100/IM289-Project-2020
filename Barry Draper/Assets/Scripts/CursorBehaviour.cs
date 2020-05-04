/*****************************************************************************
// File Name : CursorBehaviour
// Author : Kyle Grenier
// Creation Date : May 03, 2020
//
// Brief Description : Behaviour for switching between cursors
*****************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField]
    private CursorBehaviour previousCursor = null;
    [SerializeField]
    private CursorBehaviour nextCursor = null;

    [SerializeField]
    private int index = -1;

    //Read-only. Returns true if the cursor is active.
    private bool active;

    //The image component. Used to hide/show the cursor.
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Start()
    {
        //Assig index if it has not been manually assigned.
        if (index < 0)
        {
            if (previousCursor)
                index = previousCursor.index + 1;
            else
                index = 0;
        }

        //Deactivate on start.
        Deactivate();
    }




    //Make the image visible.
    public void SetActive()
    {
        img.enabled = true;
        active = true;
    }

    //Make the image invisible.
    public void Deactivate()
    {
        img.enabled = false;
        active = false;
    }

    //If there is a next cursor, deactivate the current one and activate the next.
    //Return this cursor if there is no next.
    public CursorBehaviour ActivateNext()
    {
        if (nextCursor)
        {
            Deactivate();
            nextCursor.SetActive();
            return nextCursor;
        }

        return this;
    }

    //If there is a previous cursor, deactivate the current one and activate the previous.
    //Return this cursor if there is no previous.
    public CursorBehaviour ActivatePrevious()
    {
        if (previousCursor)
        {
            Deactivate();
            previousCursor.SetActive();
            return previousCursor;
        }

        return this;
    }

    public bool IsActive()
    {
        return active;
    }

    public int GetIndex()
    {
        return index;
    }
}
