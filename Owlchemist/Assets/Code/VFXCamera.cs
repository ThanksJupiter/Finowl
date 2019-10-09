using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCamera : MonoBehaviour
{
    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        camera.Render();
    }
}
