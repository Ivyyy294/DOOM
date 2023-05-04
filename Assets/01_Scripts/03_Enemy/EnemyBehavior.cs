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
		APPROACH,
		ATTACK,
		TAKEDAMAGE,
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

	//Private Values
	bool playerInSight = false;
	float attackDelayTimer = 0f;
	float currentHealth;
	public float maxHealth;
	int currentWaypoint = 0;

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
		GetComponent<Animator>()?.SetTrigger("TakeDamage");

		if (currentHealth <= 0f)
			currentState = EnemyState.DYING;
		else
			currentState = EnemyState.IDLE;
		//throw new NotImplementedException();
	}

	void Start()
	{
		currentHealth = maxHealth;
		currentWaypoint = 0;
	}

	// Update is called once per frame
	void Update()
    {
		playerInSight = IsPlayerInSight();

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
			case EnemyState.ATTACK:
				Attack();
				break;
			case EnemyState.TAKEDAMAGE:
				TakeDmg();
				break;
			case EnemyState.DYING:
				Dying();
				break;
			default:
				break;
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
			if (navMeshAgent.hasPath && navMeshAgent.remainingDistance < 0.5f)
			{
				currentWaypoint++;

				if (currentWaypoint >= patrollingRoute.waypoints.Length)
					currentWaypoint = 0;

			}
			navMeshAgent.SetDestination (patrollingRoute.waypoints[currentWaypoint].position);
		}
	}

	void Attack()
	{
		if (!playerInSight)
			currentState = EnemyState.IDLE;
		else
		{
			attackDelayTimer -= Time.deltaTime;

			if (attackDelayTimer < 0f)
			{
				GetComponent<Animator>()?.SetTrigger("Attack");
				attackDelayTimer = attackDelay;
			}
		}
	}

	void Idle()
	{
		if (playerInSight)
			currentState = EnemyState.APPROACH;
		else
		{
			//currentWaypoint = 0;
			currentState = EnemyState.PATROL;
		}
			
	}

	void Approach()
	{
		if (!playerInSight)
		{
			navMeshAgent.SetDestination (transform.position);
			currentState = EnemyState.IDLE;
		}
		else
		{
			float distance = Vector3.Distance (transform.position, Camera.main.transform.position);

			if (distance <= attackRange)
				currentState = EnemyState.ATTACK;
			else
				navMeshAgent.SetDestination (Camera.main.transform.position);
		}
	}

	void Dying()
	{
		navMeshAgent.isStopped = true;
		GetComponent<Animator>()?.SetTrigger("Die");
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
}
