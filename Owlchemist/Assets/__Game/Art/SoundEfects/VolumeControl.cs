using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetVolume (float sliderValue)
    {
        mixer.SetFloat("ControlVolume", Mathf.Log10(sliderValue) * 20);
    }
}
