using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavTest : MonoBehaviour
{
    public NavMeshSurface surf;

    void Start()
    {
        surf.BuildNavMesh();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, surf.transform.position) >= 2f)
        {
            UpdateNavMesh();
        }
    }

    void UpdateNavMesh()
    {
        surf.RemoveData();
        surf.transform.position = transform.position;
        surf.BuildNavMesh();
    }
}
