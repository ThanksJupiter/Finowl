using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject vcam;
    //public bool camDone;

    private void Start()
    {
        vcam.SetActive(false);
        //camDone = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            vcam.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Capsule")
        {
            vcam.SetActive(false);
            //camDone = true;
        }
    }


}
