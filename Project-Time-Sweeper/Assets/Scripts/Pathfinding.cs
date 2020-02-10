using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grids grids;

    public void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grids = GetComponent<Grids>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    public IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSucces = false;

        Node startNode = grids.nodeWorldPos(startPos);
        Node targetNode = grids.nodeWorldPos(targetPos);

        if (startNode.canWalk && targetNode.canWalk)
        {
            HeapOpt<Node> openSet = new HeapOpt<Node>(grids.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path Found" + sw.ElapsedMilliseconds + "ms");
                    pathSucces = true;
                    break;
                }

                foreach (Node getNodes in grids.GetNodes(currentNode))
                {
                    if (!getNodes.canWalk || closedSet.Contains(getNodes))
                    {
                        continue;
                    }

                    int newMovementCostToNode = currentNode.gCost + getDist(currentNode, getNodes);
                    if (newMovementCostToNode < getNodes.gCost || !openSet.Contains(getNodes))
                    {
                        getNodes.gCost = newMovementCostToNode;
                        getNodes.hCost = getDist(getNodes, targetNode);
                        getNodes.parent = currentNode;

                        if (!openSet.Contains(getNodes))
                        {
                            openSet.Add(getNodes);
                        }
                    }
                }
            }
            yield return null;
            if (pathSucces)
            {
                waypoints = RetracePath(startNode, targetNode);
            }
            requestManager.FinishedProccesingPath(waypoints, pathSucces);
        }

    }


    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints =  simplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] simplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirOld = Vector2.zero;
        
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 dirNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(dirNew !=  dirOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            dirOld = dirNew;
        }
        return waypoints.ToArray();
    }

    int getDist(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);
    }
}