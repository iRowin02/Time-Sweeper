using UnityEngine;
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

	[Header("Mesh")]
	public float meshRes;
	public MeshFilter viewMeshFilter;
	private Mesh viewMesh;

    private float findTargetDelay;

	void Start() 
    {
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;

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

	public void DrawFOV()
	{
		int rayCount = Mathf.RoundToInt(viewAngle * meshRes);
		float rayDegrees = viewAngle / rayCount;
		List<Vector3> viewPoints = new List<Vector3>();

		for (int i = 0; i < rayCount; i++)
		{
			float angle = transform.eulerAngles.y - viewAngle/2 + rayDegrees * i;
			ViewCastInfo newViewCastInfo = ViewCast(angle);
			viewPoints.Add(newViewCastInfo.point);
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;

		for (int i = 0; i < vertexCount -1; i++)
		{
			vertices[i+1] = viewPoints[i];
			if (i < vertexCount -2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}

	ViewCastInfo ViewCast(float globalAngle)
	{
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit hit;

		if(Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
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
		if(!angleIsGlobal) 
        {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
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
}