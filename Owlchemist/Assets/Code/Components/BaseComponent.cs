using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseComponent : MonoBehaviour
{
    public GameObject GetOwnerGO()
    {
        return transform.gameObject;
    }
}
