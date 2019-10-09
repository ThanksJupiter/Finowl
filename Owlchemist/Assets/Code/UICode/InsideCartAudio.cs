using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideCartAudio : MonoBehaviour
{
    public AudioClip door;
    public AudioClip craft;
    public AudioClip[] page;
    AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Craftsound()
    {
        aS.Stop();
        aS.PlayOneShot(craft);
    }
    public void Pagesound()
    {
        aS.Stop();
        aS.PlayOneShot(page[Random.Range(0, page.Length - 1)]);
    }
    public void Doorsound()
    {
        aS.Stop();
        aS.PlayOneShot(door);
    }
}
