using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowningBehaviour : MonoBehaviour
{
    float time = 0;


    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "water")
        {
            time += Time.deltaTime;
        }

        if(time >= 10)
        {
            GameObject gc = GameObject.Find("Game Controller");

            gc.GetComponent<GameControllerScript>().RemoveLivesFromPlayer(1);            
        }
    }
}
