using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowningBehaviour : MonoBehaviour
{
    private float time = 0;
    public float breathTime = 10;

    public void CheckHeight(Transform maxHeight)
    {
        //print(transform.position.y - maxHeight.position.y);

        //If the player is below the water level, drown.
        if (transform.position.y - maxHeight.position.y < 0f)
            Drown();
        else if (time != 0)
            time = 0;
    }

    private void Drown()
    {
        time += Time.deltaTime;
        if (time >= breathTime)
        {
            GameControllerScript.instance.RemoveLivesFromPlayer(1);
            time = 0;
        }
    }
}
