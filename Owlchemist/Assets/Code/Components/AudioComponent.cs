using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : BaseComponent
{
    

    public AudioSource oneShotSource;

    [Header("Player")]

    public AudioClip torchSwipe;
    public AudioClip torchIdle;
    public AudioClip torchOn;
    public AudioClip torchOff;

    public AudioClip[] footsteps;

    public AudioClip harvesFlower;
    public AudioClip harvesSchroom;

    [Header("Cart")]

    public AudioClip cartCraft;
    public AudioClip completQuest;
    public AudioClip newQuest;

    public AudioClip[] pageTurns;

    public AudioClip door;

    public delegate void PlayTorchToggleSound();
    public PlayTorchToggleSound OnPlayTorchToggleSound;

    private void Awake()
    {
        oneShotSource = GetComponent<AudioSource>();
    }
}
