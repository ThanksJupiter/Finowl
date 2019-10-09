using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSpawner : MonoBehaviour
{

    [SerializeField] GameObject bridgeRightUp;
    [SerializeField] GameObject bridgeRightDown;

    [SerializeField] GameObject bridgeMiddleUp;
    [SerializeField] GameObject bridgeMiddleDown;

    [SerializeField] GameObject bridgeTopUp;
    [SerializeField] GameObject bridgeTopDown;

    [SerializeField] GameObject bridgeTopRightUp;
    [SerializeField] GameObject bridgeTopRightDown;

    [SerializeField] PlayerEventComponent eventComponent;

    private void Awake()
    {
        eventComponent.OnSisterCleansed += RaiseBridges;
    }

    public void RaiseBridges()
    {
        bridgeRightUp.gameObject.SetActive(true);
        bridgeMiddleUp.gameObject.SetActive(true);
        bridgeTopUp.gameObject.SetActive(true);
        bridgeTopRightUp.gameObject.SetActive(true);

        bridgeRightDown.gameObject.SetActive(false);
        bridgeMiddleDown.gameObject.SetActive(false);
        bridgeTopDown.gameObject.SetActive(false);
        bridgeTopRightDown.gameObject.SetActive(false);
    }

}
