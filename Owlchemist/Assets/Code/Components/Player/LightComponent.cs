using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightComponent : BaseComponent
{
    public bool startLit = false;

    public int maxCharges = 3;

    public int currentCharges { get; set; }

    public Light lightSource;
    public GameObject flames;
    public ParticleSystem fizzleSystem;
    public float lightIntensity;
    public float maxFuel = 100f;
    public float currentFuelDecay { get; set; }
    public float fuelDecay = 10f;
    public float runningFuelDecay = 6f;
    public float increasedFuelDecay = 30f;

    public float maxIntensity = 2f;
    public float minIntensity = .5f;

    public AudioSource source;
    public AudioClip activetorchSoundLoop;
    private ParticleSystem flameSystem;

    public List<Light> nearbyLights = new List<Light>();
    bool cartbool = false;
    public float currentFuel;
    private GameManagerComponent playerRef;
    private void Update()
    {
        if (source)
        {
            if (playerRef.isInsideCart)
            {
                source.Stop();
                cartbool = true;
            }
            else if (cartbool)
            {
                if (lightSource.enabled)
                {
                    source.Play();
                }
                cartbool = false;
            }
        }
    }

    private void Awake()
    {
        lightSource.enabled = startLit;
        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManagerComponent>();
        flames.SetActive(true);

        lightIntensity = lightSource.intensity;
        currentFuel = 0;
        if (source)
        {
            source.clip = activetorchSoundLoop;
            source.loop = true;
        }

        currentCharges = maxCharges;
        flameSystem = flames.GetComponent<ParticleSystem>();

        currentCharges = 0;
    }

    public void Reset()
    {
        currentFuel = maxFuel;
    }

    public bool IsLightEnabled()
    {
        return lightSource.enabled;
    }

    public void PlayFizzle()
    {
        fizzleSystem.Play();
    }

    public void SetLightEnabled(bool value)
    {
        lightSource.enabled = value;

        if (value)
        {
            flameSystem.Play();
            if (source)
            {
                source.Play();
            }
        }
        else
        {
            flameSystem.Stop();
            if (source)
            {
                source.Stop();
            }
        }
    }


}
