using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridComponent : MonoBehaviour
{
    public LayerMask obstacleMask;
    public int xResolution = 12;
    public int yResolution = 12;
    public float nodeRadius = 1f;

    GridNode[,] grid;

    float nodeDiameter;

    Vector3 worldBottomLeft;

    private void Awake()
    {
        
    }

    private void Start()
    {
        CreateGrid();
    }

    private void Update()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        nodeDiameter = nodeRadius * 2;
        grid = new GridNode[xResolution, yResolution];

        float halfX = nodeRadius * (xResolution * 0.5f);
        float halfY = nodeRadius * (yResolution * 0.5f);

        worldBottomLeft = transform.position - Vector3.right * halfX * (xResolution * 0.5f) -
            Vector3.forward * halfY * (yResolution * 0.5f);

        for (int x = 0; x < xResolution; x++)
        {
            for (int y = 0; y < yResolution; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius));
                grid[x, y] = new GridNode(walkable, worldPoint);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            Gizmos.color = Color.red;
            foreach (GridNode node in grid)
            {
                Gizmos.DrawWireSphere(node.worldPosition, .3f);
            }
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(worldBottomLeft, .5f);
    }
}
