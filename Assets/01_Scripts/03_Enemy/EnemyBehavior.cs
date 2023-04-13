using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class EnemyBehavior : MonoBehaviour
{
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
	[SerializeField] float sightRange = 25f;
	[SerializeField] float attackDelay = 2f;

	bool playerInSight = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
			attackDelay -= Time.deltaTime;

			if (attackDelay < 0f)
			{
				GetComponent<Animator>()?.SetTrigger("Attack");
				attackDelay = 2f;
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
		Ray ray = new Ray(transform.position, (Camera.main.transform.position - transform.position).normalized);
		
		RaycastHit hit;
		bool inRange = false;

		if (Physics.Raycast(ray, out hit, sightRange))
			inRange = hit.collider.CompareTag ("Player");

		Debug.DrawRay (ray.origin, ray.direction * sightRange, inRange ? Color.green : Color.red);

		return inRange;
	}
}
