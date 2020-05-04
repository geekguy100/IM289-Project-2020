/*****************************************************************************
// File Name : HealthPowerupBehaviour
// Author : Kyle Grenier
// Creation Date : April 08, 2020
//
// Brief Description : Controls how much health the powerup will give the player. Also controls rotation of the object.
*****************************************************************************/

using UnityEngine;

public class HealthPowerupBehaviour : MonoBehaviour
{
    public int health = 1;
    public ParticleSystem healthParticle;
    public float destroyTime = 2f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //If lives have been awarded, destroy the power up.
            if (GameControllerScript.instance.AwardLives(health))
            {
                Instantiate(healthParticle, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject, destroyTime);
            }
        }
    }
}
