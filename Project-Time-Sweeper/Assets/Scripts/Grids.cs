using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    public LayerMask obstacles;
    public Vector2 gridWorldSize;
    public float nodeSize;
    Node[,] grid;

    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    public void Start() 
    {
        nodeDiameter = nodeSize * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid = new Node[gridSizeX,gridSizeY];
        Vector3 worldBottemLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottemLeft + Vector3.right * (x * nodeDiameter + nodeSize) + Vector3.forward * (y * nodeDiameter + nodeSize);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeSize, obstacles));
                grid[x,y] = new Node(walkable, worldPoint);
            }
        }
    }

    public Node nodeWorldPos(Vector3 worldPos)
    {
        float percentageX = (worldPos.x +  gridWorldSize.x / 2) / gridWorldSize.x;
        float percentageY = (worldPos.z +  gridWorldSize.y / 2) / gridWorldSize.y;
        percentageX = Mathf.Clamp01(percentageX);
        percentageY = Mathf.Clamp01(percentageY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentageX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentageY);
        return grid[x,y];
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        if(grid != null)
        {
            foreach(Node n in grid)
            {
                Gizmos.color = (n.canWalk) ? Color.green : Color.red;
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
