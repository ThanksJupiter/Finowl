using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : BaseComponent
{
    public int trees = 3;

    public int Gather()
    {
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();

        mr.transform.gameObject.SetActive(false);

        return trees;
    }

    //public void 
}
