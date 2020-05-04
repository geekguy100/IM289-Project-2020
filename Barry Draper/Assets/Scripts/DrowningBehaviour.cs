/*****************************************************************************
// File Name : DrowningBehaviour
// Author : Connor Riley (90%)
            Implemented the Drowning Bar (4/30/2020)
            Kyle Grenier(10%)
                Implemented the actual Drowning (2/29/2020)
// Creation Date : February 12, 2020
//
// Brief Description : Script to control drowning under water as well as 
                       the bar that shows how much air you have left. 
*****************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class DrowningBehaviour : MonoBehaviour
{
    private float time = 0;
    public float health;
    public float breathTime = 1000;
    public Slider drownbar;

    private void Start()
    {
        drownbar.gameObject.SetActive(false);
        health = breathTime;
        drownbar.maxValue = breathTime;
        drownbar.value = health;
    }

    public void CheckHeight(Transform maxHeight)
    {
        //print(transform.position.y - maxHeight.position.y);

        //If the player is below the water level, drown.
        if (transform.position.y - maxHeight.position.y < 0f)
            Drown();
        else if (time != 0)
        {
            time = 0;
        }
            
    }

    private void Drown()
    {
        drownbar.gameObject.SetActive(true);
        health -= Time.deltaTime;
        drownbar.value = health;
        if (time >= health)
        {
            GameControllerScript.instance.RemoveLivesFromPlayer(1);
            time = 0;
        }
    }
}
