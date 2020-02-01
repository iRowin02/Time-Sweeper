using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grids grids;

    public void Awake() 
    {
        grids = GetComponent<Grids>();
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grids.nodeWorldPos(startPos);
        Node targetNode = grids.nodeWorldPos(targetPos);
    }
}
