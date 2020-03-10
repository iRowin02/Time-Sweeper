using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshTest : MonoBehaviour
{
    public Transform t;
    public NavMeshAgent agent;
    
    public void Start() 
    {
        agent.SetDestination(t.position);
    }
}
