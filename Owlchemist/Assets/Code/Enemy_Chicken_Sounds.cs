using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chicken_Sounds : MonoBehaviour
{
    StalkPlayer sP;
    AttackPlayer aP;
    AudioSource source;
    bool closed = false;
    public AudioClip huntingSound;
    public AudioClip attackSound;
    public AudioClip trigerSound;
    // Start is called before the first frame update
    void Start()
    {
        sP = GetComponent<StalkPlayer>();
        aP = GetComponent<AttackPlayer>();
        source = GetComponent<AudioSource>();
        source.pitch = Random.Range(0.8f, 3f);            
    }

    // Update is called once per frame
    void Update()
    {
        if (sP.isStalking)
        {
            if(!closed)
            {
                closed = true;
                source.clip = trigerSound;
                source.loop = false;
                source.Play();
            }
        }
        else
        {
            closed = false;
        }
        if(aP.hasAttacked)
        {
            source.clip = attackSound;
            source.loop = false;
            source.Play();
        }
        
        else if(!source.isPlaying)
        {
            LoopSong();
        }
        /*
        if(sP.isStalking)
        {
            Debug.Log("Stålmanen");
            source.Play();
        }
        else
        {
            Debug.Log("Läderlapen");
            source.Stop();
        }
        */
    }
    void LoopSong()
    {
            source.clip = huntingSound;
            source.loop = true;
            if (Random.Range(-1f, 1f) >= 0)
            {
                source.Play();
            }
    }
}
