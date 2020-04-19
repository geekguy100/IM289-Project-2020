/*****************************************************************************
// File Name : WireBehaviour
// Author : Kyle Grenier
// Creation Date : April 19, 2020
//
// Brief Description : Behaviour for the wires that connect buttons and interactables
*****************************************************************************/

using UnityEngine;

public class WireBehaviour : MonoBehaviour
{
    private SpriteRenderer sr;

    //The next wire in the string of wires. Used if bends in the wire are desired.
    public WireBehaviour nextWire;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        sr.color = color;

        //If there is a next wire in the string, change its color as well.
        if (nextWire != null)
            nextWire.ChangeColor(color);
    }
}
