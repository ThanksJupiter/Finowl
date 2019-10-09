using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshComponent : BaseComponent
{
    public GameObject surfaceObject;
    public float distanceThreshold = 3f;

    [HideInInspector]
    public NavMeshSurface surface;

    public delegate void UpdateNavMeshDelegate();
    public UpdateNavMeshDelegate OnUpdateNavMesh;

    public void Awake()
    {
        surface = surfaceObject.GetComponent<NavMeshSurface>();
    }
}
