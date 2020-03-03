﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
	[Header("General Values")]
	public float health;
    public GameObject weapon;
    public float minDist, maxDist;

    [Header("View Values")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    [Header("Movement Values")]
    public float speed;
    public float waitTime;
    public float turnSpeed;

    [Header("Layer masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Mesh")]
    [Range(0, 50)]
    public float meshRes;
    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;

    [Header("Chase")]
    public Transform _target;
    private Vector3[] paths;
    private int targetIndex;

    [Header("Patrol")]
    public Transform pathholder;
    public Vector3 lastSeen;

    private float findTargetDelay;
    private Transform player;

    private bool hasDone;
    private bool done;

    public bool inSight;

    [Header("AI States")]
    public _AIstates states;
    public _AIstates defaultState;

    public AudioClip test;

    public enum _AIstates
    {
        Idle,
        Attack,
        Patrol,
        Chase,
    }

    void Start()
    {
        AudioManager.PlaySound(test, AudioManager.AudioGroups.GameMusic);

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        inSight = false;

        findTargetDelay = 0.2f;
        StartCoroutine("FindTargetsWithDelay", findTargetDelay);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update() 
    {
        StateManager();
    }

    public void StateManager()
    {
        if(states == _AIstates.Patrol)
        {
            StopCoroutine("Patroling");
            StartCoroutine("Patroling");
            print("boi");
        }
        if(states == _AIstates.Attack)
        {
            if(_target != null)
            {
                Attack();
            }
        }
    }

	//Chase State
    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            paths = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
		Vector3 currentWaypoint = paths[0];

		while(true)
		{
			if(transform.position == currentWaypoint)
			{
				targetIndex++;
				if(targetIndex >= paths.Length)
				{
					yield break;
				}
				currentWaypoint = paths[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
			yield return null;
		}
    }
	//End Chase State

	public void Attack()
	{
        if(!inSight)
        {
            PathRequestManager.RequestPath(transform.position, lastSeen, OnPathFound);
            return;
        }

		StartCoroutine(TurnToFace(_target.position));
        weapon.GetComponent<FireArms>().Shoot();
	}

    //Idle State
    public void Idle()
    {
        //Insert animation
    }
    //End Idle State

    //Patrol State
    public void Patroling()
    {
        if (!hasDone == true)
        {
            Vector3[] waypoints = new Vector3[pathholder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathholder.GetChild(i).position;
                waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
            }
            hasDone = true;
            StartCoroutine(FollowPath(waypoints));
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        int targetWaypointInt = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointInt];

        if (transform.position != waypoints[0] && done == false)
        {
            PathRequestManager.RequestPath(transform.position, _target.position, OnPathFound);
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0], speed * Time.deltaTime);
            done = true;
        }

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointInt = (targetWaypointInt + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointInt];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    public IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirLookAt = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirLookAt.z, dirLookAt.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
    //End Patrol state

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        _target = null;
        inSight = false;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                    _target = target;
                    lastSeen = target.position;
                    inSight = true;
                }
            }
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathholder.GetChild(0).position;
        Vector3 prevPosition = startPosition;

		if(paths != null)
		{
			for (int i = targetIndex; i < paths.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(paths[i], Vector3.one);

				if(i == targetIndex)
				{
					Gizmos.DrawLine(transform.position, paths[i]);
				}
				else
				{
					Gizmos.DrawLine(paths[i - 1], paths[i]);
				}
			}
		}

        foreach (Transform waypoint in pathholder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }
        Gizmos.DrawLine(prevPosition, startPosition);
    }
}