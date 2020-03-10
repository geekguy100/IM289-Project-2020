/*****************************************************************************
// File Name : GameManagerAudioController
// Author : Kyle Grenier
// Creation Date : March 10, 2020
//
// Brief Description : Manages playing SFX related to the game manager.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAudioController : MonoBehaviour
{
    private AudioSource audioSource;

    public enum SFX { playerHit };
    public AudioClip[] audioClips;


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //If the number of audio clips does not match the number of SFX in the SFX enum, log a warning.
        if (audioClips.Length != System.Enum.GetValues(typeof(SFX)).Length)
            Debug.LogWarning("WARNING: The number of audio clips does not match the number of desired SFX -- " + transform.parent.name);
    }

    public void PlayClip(SFX clip, bool loop = false, bool randomTime = false)
    {
        if (audioSource.isPlaying && loop)
            return;

        audioSource.clip = audioClips[(int)clip];

        audioSource.loop = loop;

        if (randomTime)
            audioSource.time = Random.Range(0, audioSource.clip.length);
        else
            audioSource.time = 0f;

        print("Playing clip: " + audioSource.clip.name);
        audioSource.Play();
    }

    //Stops a loop from playing.
    public void StopPlayingLoop()
    {
        if (audioSource.isPlaying && audioSource.loop)
            audioSource.Stop();
    }
}
