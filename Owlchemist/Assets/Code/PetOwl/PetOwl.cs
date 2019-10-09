using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetOwl : MonoBehaviour
{
    public float minTime = 2f;
    public float maxTime = 5f;
    private Animator animator;
    private float currentTime = 0f;
    private float nextTrigger = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > nextTrigger)
        {
            animator.SetTrigger("LookAround");
            currentTime = 0f;
            nextTrigger = Random.Range(minTime, maxTime);
        }
    }
}
