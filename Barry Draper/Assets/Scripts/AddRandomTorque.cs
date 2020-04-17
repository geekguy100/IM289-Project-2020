/*****************************************************************************
// File Name : AddRandomTorque
// Author : Kyle Grenier
// Creation Date : April 17, 2020
//
// Brief Description : Adds a random torque to a rigidbody.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomTorque : MonoBehaviour
{
    private Rigidbody2D rb;
    /// <summary>
    /// The min and max torque, respectively.
    /// </summary>
    public Vector2 torque;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float t = Random.Range(torque.x, torque.y);
        rb.AddTorque(t, ForceMode2D.Impulse);
    }
}
