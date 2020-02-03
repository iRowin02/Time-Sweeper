using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker;
    public Transform target;

    Grids grids;

    public void Awake() 
    {
        grids = GetComponent<Grids>();
    }

    public void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grids.nodeWorldPos(startPos);
        Node targetNode = grids.nodeWorldPos(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode); 

            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node getNodes in grids.GetNodes(currentNode))
            {
                if(!getNodes.canWalk || closedSet.Contains(getNodes))    
                {
                    continue;
                }

                int newMovementCostToNode = currentNode.gCost + getDist(currentNode, getNodes);
                if(newMovementCostToNode < getNodes.gCost || !openSet.Contains(getNodes))
                {
                    getNodes.gCost = newMovementCostToNode;
                    getNodes.hCost = getDist(getNodes, targetNode);
                    getNodes.parent= currentNode;

                    if(!openSet.Contains(getNodes))
                    {
                        openSet.Add(getNodes);
                    }
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grids.path = path;
    }

    int getDist(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(distX > distY)
        {
            return 14*distY + 10* (distX - distY);
        }
        return 14*distX + 10* (distY - distX);
    }
}