using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface surface;
    public float distanceThreshold = 5f;

    void Awake()
    {
        surface.BuildNavMesh();
        //surface.BuildNavMesh();
        UpdateNavMesh();
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, surface.transform.position) >= distanceThreshold)
            UpdateNavMesh(); 
    }

    void UpdateNavMesh()
    {
        surface.RemoveData();
        surface.transform.position = transform.position;
        surface.BuildNavMesh();
    }
}
