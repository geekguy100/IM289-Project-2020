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
    public float rotationSpeed = 500;
    public ParticleSystem healthParticle;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //If lives have been awarded, destroy the power up.
            if (GameControllerScript.instance.AwardLives(health))
            {
                Instantiate(healthParticle, gameObject.transform.position, gameObject.transform.rotation);
                Explode();
            }
        }
    }

    void Explode()
    {
        //Instantiate our one-off particle system
        ParticleSystem explosionEffect = Instantiate(healthParticle)
                                         as ParticleSystem;
        explosionEffect.transform.position = transform.position;
        //play it
        explosionEffect.Play();


        float duration = explosionEffect.main.duration;
        //destroy the particle system when its duration is up, right
        //it would play a second time.
        Destroy(explosionEffect.gameObject, duration);
        //destroy our game object
        Destroy(this.gameObject);

    }
}
