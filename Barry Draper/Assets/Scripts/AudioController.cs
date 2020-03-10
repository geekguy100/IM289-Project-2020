/*****************************************************************************
// File Name : AudioController
// Author : Kyle Grenier
// Creation Date : March 10, 2020
//
// Brief Description : Manages playing SFX and BG music.
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [Header("SFX")]
    private AudioSource audioSource;

    public enum Type {PlayerAudio, GameManagerAudio, turretAudio, backgroundMusic};
    public Type audioType;


    public enum PlayerSFX {playerWalk, pickupBox, dropBox, openUmbrella, closeUmbrella};
    public enum GameManagerSFX { playerHit };
    public enum TurretSFX { targetFound, shoot, die};
    public AudioClip[] sfxClips;

    [Header("Background Music")]
    public AudioClip intro;
    public AudioClip loop;



    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        bool warning = false;

        //If the number of audio clips does not match the number of SFX in the PlayerSFX enum, log a warning.
        if (audioType == Type.PlayerAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(PlayerSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.GameManagerAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(GameManagerSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.turretAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(TurretSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.backgroundMusic)
        {
            StartCoroutine(PlayBGMusic());
        }
        

        if (warning)
            Debug.LogWarning("WARNING: The number of audio clips does not match the number of desired SFX -- " + transform.parent.name);
    }

    private void PlayClip(int clip, bool loop = false, bool randomTime = false)
    {
        if (audioSource.isPlaying && loop)
            return;

        audioSource.clip = sfxClips[clip];

        audioSource.loop = loop;

        if (randomTime)
            audioSource.time = Random.Range(0, audioSource.clip.length);
        else
            audioSource.time = 0f;

        print(transform.parent.name + " is playing clip: " + audioSource.clip.name);
        audioSource.Play();
    }

    //Stops a loop from playing.
    public void StopPlayingLoop()
    {
        if (audioSource.isPlaying && audioSource.loop)
            audioSource.Stop();
    }



    private IEnumerator PlayBGMusic()
    {
        if (audioType != Type.backgroundMusic)
        {
            Debug.LogWarning(transform.parent.name + " is trying to play background music!");
            yield break;
        }

        audioSource.clip = intro;
        audioSource.loop = false;
        audioSource.Play();

        //Wait until the intro is over.
        while(audioSource.time < audioSource.clip.length)
        {
            yield return null;
        }

        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }







    public void PlayClip(PlayerSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }

    public void PlayClip(GameManagerSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }

    public void PlayClip(TurretSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }
}