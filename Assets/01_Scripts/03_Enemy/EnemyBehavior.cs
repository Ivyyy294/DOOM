using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;

public class EnemyBehavior : MonoBehaviour
{
	//Public Values
	public enum EnemyState
	{
		IDLE,
		PATROL,
		APPROACH,
		ATTACK,
		TAKEDAMAGE,
		DEAD
	}

	public EnemyState currentState = EnemyState.IDLE;

	[Header ("Spotting Setting")]
	[SerializeField] Transform rayOrigin;
	[SerializeField] float rayRange = 25f;

	[Header ("Attack Settings")]
	[SerializeField] float attackDelay = 2f;

	//Private Values
	bool playerInSight = false;
	float attackDelayTimer = 0f;

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
				break;
			case EnemyState.ATTACK:
				Attack();
				break;
			default:
				break;
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
			currentState = EnemyState.ATTACK;
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
