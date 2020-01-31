using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask obstacles;
    public Vector2 gridWorldSize;
    public float nodeSize;
    Node[,] grid;

     void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1,gridWorldSize.y));
    }
}
