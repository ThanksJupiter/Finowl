using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CMIntroCamera : MonoBehaviour
{
    public GameObject vcam1;
    public GameObject vcam2;
    public GameObject vcam3;
    public GameObject vcam4;

    public void CameraChange()
    {
        if (vcam1.activeSelf == true)
        {
            CamerasFalse();
            vcam2.SetActive(true);
        }

        else if (vcam2.activeSelf == true)
        {
            CamerasFalse();
            vcam3.SetActive(true);
        }
        else if (vcam3.activeSelf == true)
        {
            CamerasFalse();
            vcam4.SetActive(true);
        }
        else if (vcam4.activeSelf == true)
        {
            CamerasFalse();
            vcam1.SetActive(true);
        }
    }

    private void CamerasFalse()
    {
        vcam1.SetActive(false);
        vcam2.SetActive(false);
        vcam3.SetActive(false);
        vcam4.SetActive(false);
    }
}
