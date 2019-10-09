using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Light))]
public class TorchComponent : BaseComponent
{
    public Light torchLight;

    private void Awake()
    {
        torchLight.enabled = false;
    }
}
