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

    //Is the wire a wire circle? If so, then the alpha channel in the color will be 100%.
    public bool wireCircle;

    //The next wire in the string of wires. Used if bends in the wire are desired.
    public WireBehaviour nextWire;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color color)
    {
        if (wireCircle)
        {
            Color c = color;
            c.a = 1;
            sr.color = c;
        }
        else
            sr.color = color;


        //If there is a next wire in the string, change its color as well.
        if (nextWire != null)
            nextWire.ChangeColor(color);
    }
}
