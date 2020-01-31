using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool canWalk;
    public Vector3 worldPos;

    public Node(bool _canWalk, Vector3 _worldPos)
    {
        canWalk = _canWalk;
        worldPos = _worldPos;
    }
}
