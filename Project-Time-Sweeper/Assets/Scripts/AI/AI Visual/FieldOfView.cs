﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour 
{
    [Header("View Values")]
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;


    [Header("Layer masks")]
	public LayerMask targetMask;
	public LayerMask obstacleMask;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();

    private float findTargetDelay;

	void Start() 
    {
        findTargetDelay = 0.2f;
		StartCoroutine ("FindTargetsWithDelay", findTargetDelay);
	}

	IEnumerator FindTargetsWithDelay(float delay) 
    {
		while(true) 
        {
			yield return new WaitForSeconds(delay);
			FindVisibleTargets ();
		}
	}

	void FindVisibleTargets() 
    {
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

		for(int i = 0; i < targetsInViewRadius.Length; i++) 
        {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) 
            {
				float distToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics.Raycast (transform.position, dirToTarget, distToTarget, obstacleMask)) 
                {
					visibleTargets.Add (target);
				}
			}
		}
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) 
    {
		if(!angleIsGlobal) 
        {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}