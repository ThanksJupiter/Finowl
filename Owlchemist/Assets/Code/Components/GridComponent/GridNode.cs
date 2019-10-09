using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridNode
{
    public bool walkable;
    public Vector3 worldPosition;

    public GridNode(bool walkable, Vector3 worldPosition)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
    }
}
