using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grids : MonoBehaviour
{
    public bool showGizmos;

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

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
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
                grid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNodes(Node node)
    {
        List<Node> getNodes = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
           for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;    
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    getNodes.Add(grid[checkX, checkY]);
                }
            } 
        }
        return getNodes;
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

    public List<Node> path;

    void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        if(showGizmos)
        {
            if(path != null)
            {
                foreach(Node n in path)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
        else
        { 
            if(grid != null)
            {
                foreach(Node n in grid)
                {
                    Gizmos.color = (n.canWalk) ? Color.green : Color.red;
                    if(path != null)
                    {
                        if(path.Contains(n))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
                }
            }
        }
    }
}