/*****************************************************************************
// File Name : MovingBackground
// Author : Kyle Grenier
// Creation Date : April 20, 2020
//
// Brief Description : Pans the background on the menu.
*****************************************************************************/

using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public RectTransform bg;
    public float movementSpeed = 1f;

    private bool moveRight = false;

    public float minX = -140f;
    public float maxX = 900f;

    public void Update()
    {
        Vector3 pos = bg.position;

        if (bg.position.x < minX)
            moveRight = true;
        else if (bg.position.x > maxX)
            moveRight = false;

        if (moveRight)
            pos.x += movementSpeed * Time.deltaTime;
        else
            pos.x -= movementSpeed * Time.deltaTime;

        bg.position = pos;
    }
}
