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
    private AudioSource audioSource;

    public enum Type {PlayerAudio, GameManagerAudio, turretAudio, backgroundMusic, fanAudio, buttonAudio, minionAudio, bossAudio};

    [Header("SFX")]
    public Type audioType;


    public enum PlayerSFX {playerWalk, pickupBox, dropBox, openUmbrella, closeUmbrella};
    public enum MinionSFX { minionWalk, minionShoot, minionHit, minionDie };
    public enum GameManagerSFX { playerHit, gameOver, finishLevel };
    public enum TurretSFX { targetFound, shoot};
    public enum FanSFX { fanWhir };
    public enum ButtonSFX { buttonOn, buttonOff };
    public enum BossSFX { hit, whistleBlow, shoot, jump, smash, die};
    public AudioClip[] sfxClips;

    [Header("Background Music")]
    public AudioClip intro;
    public AudioClip loop;

    private bool bgMusic = false;

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
        else if (audioType == Type.fanAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(FanSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.buttonAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(ButtonSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.minionAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(MinionSFX)).Length)
                warning = true;
        }
        else if (audioType == Type.bossAudio)
        {
            if (sfxClips.Length != System.Enum.GetValues(typeof(BossSFX)).Length)
                warning = true;
        }
        

        if (warning)
            Debug.LogWarning("WARNING: The number of audio clips does not match the number of desired SFX -- " + transform.parent.name);
    }

    //Internal use only. Public functions are listen at the bottom of the script.
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

       // print(transform.parent.name + " is playing clip: " + audioSource.clip.name);
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

        if (!bgMusic)
            yield break;

        audioSource.clip = intro;
        audioSource.loop = false;
        audioSource.Play();

        //Wait until the intro is over.
        yield return new WaitForSeconds(audioSource.clip.length);

        if (!bgMusic)
            yield break;

        audioSource.clip = loop;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        if (audioType != Type.backgroundMusic)
        {
            Debug.LogWarning("Only the child of the GameManager, BG Music Source, has the right to stop the background music from playing.");
            return;
        }

        print("Stopping BG Music");
        bgMusic = false;
        audioSource.Stop();
        StopAllCoroutines();

    }

    public void PlayBackgroundMusic()
    {
        bgMusic = true;
        StartCoroutine(PlayBGMusic());
    }




    public void PlayClip(PlayerSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }

    public void PlayClip(MinionSFX clip, bool loop = false, bool randomTime = false)
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

    public void PlayClip(FanSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }

    public void PlayClip(ButtonSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }

    public void PlayClip(BossSFX clip, bool loop = false, bool randomTime = false)
    {
        PlayClip((int)clip, loop, randomTime);
    }
}