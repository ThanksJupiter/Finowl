using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnvironmentComponent : BaseComponent
{
    private Tree[] trees;
    public NavMeshSurface navMeshSurface;

    private void Awake()
    {
        navMeshSurface = GetComponentInChildren<NavMeshSurface>();
        trees = GetComponentsInChildren<Tree>();
    }
}
