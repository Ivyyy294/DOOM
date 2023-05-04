using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
using  UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour , Damageable
{
	//Public Values
	public enum EnemyState
	{
		IDLE,
		PATROL,
		PATROL_PAUSE,
		APPROACH,
		ATTACK,
		TAKEDAMAGE,
		INVESTIGATE,
		DYING,
		DEAD
	}

	public EnemyState currentState = EnemyState.IDLE;

	[Header ("Spotting Setting")]
	[SerializeField] Transform rayOrigin;
	[SerializeField] float rayRange = 25f;

	[Header ("Attack Settings")]
	[SerializeField] float attackRange = 1f;
	[SerializeField] float attackDelay = 2f;

	[Header ("AI Settings")]
	[SerializeField] NavMeshAgent navMeshAgent;
	[SerializeField] PatrollingRoute patrollingRoute;
	[SerializeField] float idleDuration = 1f;
	[SerializeField] float idleChance = 0.25f;
	[SerializeField] float waypointAccuracy = 0.25f;

	//Private Values
	private Animator animator;
	bool playerInSight = false;
	float attackDelayTimer = 0f;
	float currentHealth;
	public float maxHealth;
	float idleTimer = 0f;
	Vector3 lastPlayerPos;

	public void ApplyDamage (float dmg)
	{
		if (currentHealth > 0)
		{
			currentHealth -= dmg;
			Debug.Log ("AUTSCH!!! DMG: " + dmg.ToString("0.00"));

			currentState = EnemyState.TAKEDAMAGE;
		}
	}

	private void TakeDmg()
	{
		animator?.SetTrigger("TakeDamage");

		if (currentHealth <= 0f)
			currentState = EnemyState.DYING;
		else
			currentState = EnemyState.IDLE;
		//throw new NotImplementedException();
	}

	void Start()
	{
		currentHealth = maxHealth;
		animator = GetComponent<Animator>();
		idleTimer = idleDuration;

		//Start at nearest point
		patrollingRoute?.SetNearestWaypoint (transform);
	}

	// Update is called once per frame
	void Update()
    {
		playerInSight = IsPlayerInSight();

		if (playerInSight)
			lastPlayerPos = Camera.main.transform.position;

        switch (currentState)
		{
			case EnemyState.IDLE:
				Idle();
				break;
			case EnemyState.APPROACH:
				Approach();
				break;
			case EnemyState.PATROL:
				Patrol();
				break;
			case EnemyState.PATROL_PAUSE:
				PatrolPause();
				break;
			case EnemyState.ATTACK:
				Attack();
				break;
			case EnemyState.TAKEDAMAGE:
				TakeDmg();
				break;
			case EnemyState.DYING:
				Dying();
				break;
			case EnemyState.INVESTIGATE:
				Investigate();
				break;
			default:
				break;
		}

		animator.SetBool ("walking", (currentState == EnemyState.APPROACH || currentState == EnemyState.PATROL));
    }

	private void Investigate()
	{
		if (playerInSight)
			currentState = EnemyState.APPROACH;
		else
		{
			navMeshAgent.SetDestination (lastPlayerPos);

			if (navMeshAgent.remainingDistance < waypointAccuracy)
			{
				currentState = EnemyState.IDLE;

				//Return to nearest waypoint
				if (patrollingRoute != null)
				{
					patrollingRoute.SetNearestWaypoint (transform);
					currentState = EnemyState.PATROL_PAUSE;
				}
			}
		}
	}

	private void PatrolPause()
	{
		navMeshAgent.SetDestination (transform.position);

		if (playerInSight)
			currentState = EnemyState.APPROACH;
		else
		{
			if (idleTimer >= idleDuration)
			{
				idleTimer = 0f;

				if (patrollingRoute != null)
					currentState = EnemyState.PATROL;
			}
			else
				idleTimer += Time.deltaTime;
		}
	}

	private void Patrol()
	{
		if (playerInSight)
		{
			currentState = EnemyState.APPROACH;
			return;
		}

		if (patrollingRoute != null)
		{
			if (navMeshAgent.hasPath && navMeshAgent.remainingDistance < waypointAccuracy)
			{
				patrollingRoute.Next();

				if (UnityEngine.Random.value <= idleChance)
				{
					currentState = EnemyState.PATROL_PAUSE;
					return;
				}
			}

			navMeshAgent.SetDestination (patrollingRoute.GetCurrentWaypoint());
		}
	}

	void Attack()
	{
		if (!playerInSight)
			currentState = EnemyState.INVESTIGATE;
		else
		{
			float distance = Vector3.Distance (transform.position, Camera.main.transform.position);

			if (distance > attackRange)
			{
				currentState = EnemyState.APPROACH;
				return;
			}

			attackDelayTimer -= Time.deltaTime;

			if (attackDelayTimer < 0f)
			{
				animator?.SetTrigger("Attack");
				attackDelayTimer = attackDelay;
			}
		}
	}

	void Idle()
	{
		navMeshAgent.SetDestination (transform.position);

		if (playerInSight)
			currentState = EnemyState.APPROACH;
		else
			currentState = EnemyState.PATROL;
	}

	void Approach()
	{
		if (!playerInSight)
			currentState = EnemyState.INVESTIGATE;
		else
		{
			if (navMeshAgent.hasPath && navMeshAgent.remainingDistance <= attackRange)
				currentState = EnemyState.ATTACK;

			navMeshAgent.SetDestination (lastPlayerPos);
		}
	}

	void Dying()
	{
		navMeshAgent.isStopped = true;
		animator?.SetTrigger("Die");
		Dead();
	}

	void Dead()
	{
		if (currentState != EnemyState.DEAD)
		{
			Debug.Log ("ME DEAD!!");
			PlayerStats.Me().enemiesKilled++;
			currentState = EnemyState.DEAD;
			//gameObject.SetActive (false);
		}
	}

	bool IsPlayerInSight()
	{
		bool inRange = false;

		Assert.IsNotNull (rayOrigin, "Enemies rayOrigin not set!");

		Ray ray = new Ray(rayOrigin.position, (Camera.main.transform.position - rayOrigin.position).normalized);
		
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, rayRange))
			inRange = hit.collider.CompareTag ("Player");

		Debug.DrawRay (ray.origin, ray.direction * rayRange, inRange ? Color.green : Color.red);

		return inRange;
	}

	//void SetNavPosition(Vector3 pos)
	//{
	//	NavMeshPath path = new NavMeshPath();
	//	navMeshAgent.CalculatePath(pos, path);

	//	if (path.status == NavMeshPathStatus.PathPartial)
	//		pos = path.corners[path.corners.Length - 1];

	//	navMeshAgent.SetDestination(pos);
	//}
}
