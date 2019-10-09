using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Assertions;

public class TimelineActivation : MonoBehaviour
{
    public PlayableDirector timeLine;
    private GameObject playerRef;

    private void Awake()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerRef, "IntroTimeline GO cannot find player reference!");
        timeLine = GetComponent<PlayableDirector>();
    }

    void Start()
    {
        timeLine.Play();
        timeLine.stopped += OnPlayableDirectorStopped;
        playerRef.GetComponent<NavMeshAgent>().enabled = false;
    }

    private void Update()
    {

    }
    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        //playerRef.GetComponent<GameManagerComponent>()?.OnStartGameTick();
        playerRef.GetComponent<NavMeshAgent>().enabled = true;
    }



}
