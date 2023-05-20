using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollController
{
	private int currentWaypoint = 0;
	private bool revers = false;
	public PatrollingRoute patrollingRoute;

	public Vector3 GetCurrentWaypoint()
	{
		if (patrollingRoute != null && currentWaypoint < patrollingRoute.waypoints.Length)
			return patrollingRoute.waypoints[currentWaypoint].position;

		return default (Vector3);
	}

	public void Next()
	{
		if (patrollingRoute == null)
			return;

		if (patrollingRoute.mode == PatrollingRoute.Mode.LOOP)
		{
			++currentWaypoint;

			if (currentWaypoint >= patrollingRoute.waypoints.Length)
				currentWaypoint = 0;
		}
		else
		{
			if (!revers)
			{
				++currentWaypoint;

				if (currentWaypoint >= patrollingRoute.waypoints.Length)
				{
					currentWaypoint -= 2;
					revers = true;
				}
			}
			else
			{
				--currentWaypoint;

				if (currentWaypoint < 0)
				{
					currentWaypoint += 2;
					revers = false;
				}
			}
		}
	}

	public void SetNearestWaypoint (Transform pos)
	{
		if (patrollingRoute == null)
			return;

		currentWaypoint = patrollingRoute.GetNearestWaypoint (pos);
	}
}

public class PatrollingRoute : MonoBehaviour
{
	[System.Serializable]
	public enum Mode
	{
		LOOP,
		PINGPONG
	}
	
	public Transform[] waypoints;
	public Mode mode;

	public int GetNearestWaypoint (Transform pos)
	{
		int target = 0;
		
		if (waypoints.Length > 0)
		{
			//Init with first waypoint
			float istDist = Vector3.Distance (pos.position, waypoints[target].position);

			for (int i = 0; i < waypoints.Length; ++i)
			{
				float tmpDist = Vector3.Distance (pos.position, waypoints[i].position);

				if (tmpDist < istDist)
				{
					target = i;
					istDist = tmpDist;
				}
			}
		}

		return target;
	}

	private void OnDrawGizmos()
	{
		if (waypoints != null)
		{
			for (int i = 0; i < waypoints.Length -1; ++i)
				Debug.DrawLine (waypoints[i].position, waypoints[i+1].position, Color.green);

			if (mode == Mode.LOOP)
				Debug.DrawLine (waypoints[0].position, waypoints[waypoints.Length-1].position, Color.green);
		}
	}
}
