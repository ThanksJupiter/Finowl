using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOnSense : MonoBehaviour
{
    public bool enter;
    private Collider boxCollider;
    public GameObject player;

    private void Awake()
    {
        Collider boxCollider = gameObject.GetComponent<Collider>();
        boxCollider.isTrigger = true;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            enter = true;
        }
    }
}
