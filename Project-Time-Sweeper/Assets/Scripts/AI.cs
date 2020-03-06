using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
	[Header("General Values")]
	public float health;
    public GameObject weapon;
    public float minDist, maxDist;

    [Header("Movement Values")]
    public float speed;
    public float waitTime;
    public float turnSpeed;

    [Header("Layer masks")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    [Header("Chase")]
    public Transform _target;
    private Vector3[] paths;
    private int targetIndex;

    [Header("Patrol")]
    public Transform pathholder;

    [Header("Line of Sight")]
    public bool inSight;
    public Vector3 lastSeen;
    public GameObject viewpoint;

    [Header("AI States")]
    public _AIstates states;

    public AudioClip test;

    public enum _AIstates
    {
        Idle,
        Attack,
        Patrol,
        Alert,
    }

    void Start()
    {
        AudioManager.PlaySound(test, AudioManager.AudioGroups.GameMusic);

        inSight = false;
    }

    public void Update() 
    {
        switch(states)
        {
            default:
            case _AIstates.Idle:
                if(pathholder == null)
                {
                    //FindVisibleTargets();
                }
                else
                {
                    states = _AIstates.Patrol;
                }
            break;

            case _AIstates.Patrol:
                if(pathholder != null)
                {
                    StartCoroutine("Patroling");
                }
            break;
            
            case _AIstates.Attack:


                transform.LookAt(_target);
                print("Im shooting");
            break;
        }
    }

	//Chase State
    public void OnPathFound(Vector3[] newPath, bool pathSuccesful)
    {
        if (pathSuccesful)
        {
            paths = newPath;
            StopCoroutine("FollowPaths");
            StartCoroutine("FollowPaths");
        }
    }

    IEnumerator FollowPaths()
    {
		Vector3 currentWaypoint = paths[0];
        StopCoroutine("TurnToFace");

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
        transform.LookAt(_target);
        print("schiet");
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
         Vector3[] waypoints = new Vector3[pathholder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathholder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        int targetWaypointInt = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointInt];

        if (transform.position != waypoints[0])
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[0], speed * Time.deltaTime);
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

    public void OnTriggerStay(Collider other) 
    {
        NoticePlayer(other.gameObject);
    }

    public void OnTriggerExit(Collider other) 
    {
        UnNoticePlayer(other.gameObject);
    }

    public void NoticePlayer(GameObject player)
    {
        RaycastHit hit;

        Physics.Raycast(viewpoint.transform.position, player.transform.position, out hit, Mathf.Infinity);

        if(hit.transform.tag == "Player")
        {
            inSight = true;
            states = _AIstates.Attack;
        }
        else
        {
            inSight = false;
        }
    }

    public void UnNoticePlayer(GameObject player)
    {
        inSight = false;

        lastSeen = player.transform.position;

        CheckLastLocation(player.transform);
    }

    public void CheckLastLocation(Transform lastKnown)
    {
        PathRequestManager.RequestPath(gameObject.transform.position, lastKnown.position, OnPathFound);
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