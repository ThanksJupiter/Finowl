using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.3f;
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 2f;

    private float shakeElapsedTime = 0f;

    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin cameraNoise;

    private GameObject[] enemy;

    private void Awake()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        Assert.IsNotNull(enemy, "Cinemachine cannot find game objects with the tag Enemy");
    }

    private void Start()
    {
        if (virtualCamera != null)
        {
            cameraNoise = virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        }
    }

    private void Update()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].GetComponent<ChickenAIComponent>().stateMachine.attacking == true)
            {
                shakeElapsedTime = shakeDuration;
            }
        }

        if (virtualCamera != null || cameraNoise != null)
        {
            if (shakeElapsedTime > 0)
            {
                cameraNoise.m_AmplitudeGain = shakeAmplitude;
                cameraNoise.m_FrequencyGain = shakeFrequency;

                shakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                cameraNoise.m_AmplitudeGain = 0f;
                shakeElapsedTime = 0f;
            }
        }

    }
}
