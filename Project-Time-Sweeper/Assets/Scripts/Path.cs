using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    Vector3[] pathNodes;
    public int currentPathIndex = 0;

    public Path(Vector3[] pathNodes)
    {
        this.pathNodes = pathNodes;
    }

    public Vector3[] GetPathNodes()
    {
        return pathNodes;
    }

    public Vector3 GetNextNode()
    {
        if (currentPathIndex < pathNodes.Length)
        {
            return pathNodes[currentPathIndex];
        }

        return Vector3.negativeInfinity; 
    }

    public bool ReachedEndNode()
    {
        return (currentPathIndex == pathNodes.Length); 
    }
}
