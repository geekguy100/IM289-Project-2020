/*****************************************************************************
// File Name : DestroyAfterTime
// Author : Kyle Grenier
// Creation Date : May 04, 2020
//
// Brief Description : Destroys a GameObject after an amount of time.
*****************************************************************************/

using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
