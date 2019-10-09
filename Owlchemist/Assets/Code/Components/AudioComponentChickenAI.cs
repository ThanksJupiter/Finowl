using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComponentChickenAI : MonoBehaviour
{
    public AudioClip[] walkingSounds;
    public AudioClip stalkingSound;
    public AudioClip runningSound;
    public AudioClip lookaroundSound;
    public AudioClip[] staggerSound;
    public AudioClip stompSound;
    int voiceNumber;
    public AudioSource audioS;

    private void Start()
    {
        voiceNumber = Random.Range(0,1);
    }
    void WalkingSound()
    {
        audioS.volume = 0.8f;
        audioS.PlayOneShot(walkingSounds[Random.Range(0, walkingSounds.Length - 1)]);
    }

    void StalkingSound()
    {
        //audioS.PlayOneShot(walkingSounds[Random.Range(0, walkingSounds.Length - 1)]);
        if (!audioS.isPlaying)
        {
            audioS.volume = 0.4f;
            audioS.PlayOneShot(stalkingSound);
        }
    }

    void RunningSound()
    {
        audioS.volume = 0.4f;
        audioS.PlayOneShot(runningSound);
    }

    void LookAroundSound()
    {
        audioS.volume = 0.4f;
        audioS.PlayOneShot(lookaroundSound);
    }

    void StaggerSound()
    {
        audioS.volume = 0.4f;
        audioS.PlayOneShot(staggerSound[voiceNumber]);
    }

    void StompSound()
    {
        audioS.volume = 0.4f;
        audioS.PlayOneShot(stompSound);
    }
}