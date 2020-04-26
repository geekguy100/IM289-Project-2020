/*****************************************************************************
// File Name : SmokeEffectBehaviour
// Author : Kyle Grenier
// Creation Date : April 26, 2020
//
// Brief Description : Controls how the smoke effect expands and contracts when spawned.
*****************************************************************************/

using System.Collections;
using UnityEngine;

public class SmokeEffectBehaviour : MonoBehaviour
{
    [Header("Speed of Effect")]
    //Speed multiplier for expansion of smoke.
    public float increaseSpeed = 1f;
    //Speed multiplier for contraction of smoke.
    public float decreaseSpeed = 1f;

    [Header("Scaling")]
    //The max scale the smoke should be.
    public Vector3 maxScale;
    //The current scale of the smoke.
    private Vector3 currentScale;

    [Header("Max Size Stall Time")]
    //The time the smoke will stall at its max size.
    public float stallTime = 0.5f;

    private void Start()
    {
        currentScale = transform.localScale;
        StartCoroutine(IncreaseSize());
    }

    //Increases the scale of the smoke over time.
    private IEnumerator IncreaseSize()
    {
        while (Vector3.Distance(currentScale, maxScale) > 0.5f)
        {
            currentScale = Vector3.Lerp(currentScale, maxScale, increaseSpeed * Time.deltaTime);
            transform.localScale = currentScale;
            yield return null;
        }

        //Wait stallTime before contracting the smoke.
        yield return new WaitForSeconds(stallTime);

        while (Vector3.Distance(currentScale, Vector3.zero) > 0.5f)
        {
            currentScale = Vector3.Lerp(currentScale, Vector3.zero, decreaseSpeed * Time.deltaTime);
            transform.localScale = currentScale;
            yield return null;
        }

        //Destroys the GameObject at the end of the effect.
        Destroy(gameObject);
    }
}