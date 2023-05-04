using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingRoute : MonoBehaviour
{
	[System.Serializable]
	enum Mode
	{
		LOOP,
		PINGPONG
	}
	
	[SerializeField] Transform[] waypoints;
	[SerializeField] Mode mode;
	private int currentWaypoint = 0;
	private bool revers = false;

	public Vector3 GetCurrentWaypoint()
	{
		if (currentWaypoint < waypoints.Length)
			return waypoints[currentWaypoint].position;

		return default (Vector3);
	}

	public void Next()
	{
		if (mode == Mode.LOOP)
		{
			++currentWaypoint;

			if (currentWaypoint >= waypoints.Length)
				currentWaypoint = 0;
		}
		else
		{
			if (!revers)
			{
				++currentWaypoint;

				if (currentWaypoint >= waypoints.Length)
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
		if (waypoints.Length > 0)
		{
			//Init with first waypoint
			int target = 0;
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

			currentWaypoint = target;
		}
	}
}
