using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("max value " + drownbar.maxValue);
        Debug.Log("value " + drownbar.value);
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
