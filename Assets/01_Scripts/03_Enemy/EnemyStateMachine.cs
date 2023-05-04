using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.AI;

public interface EnemyState
{
	public void Enter (EnemyStateMachine enemy);
	public void Update (EnemyStateMachine enemy);
}

public class IdleState : EnemyState
{
	public void Enter (EnemyStateMachine enemy)
	{
		enemy.navMeshAgent.ResetPath();
	}

	public void Update (EnemyStateMachine enemy)
	{
		enemy.SetState (EnemyStateMachine.patrole);
	}
}

public class PatroleState : EnemyState
{
	public void Enter (EnemyStateMachine enemy) {}

	public void Update (EnemyStateMachine enemy)
	{
		if (enemy.playerInSight)
			enemy.SetState (EnemyStateMachine.approach);

		if (enemy.patrollingRoute != null)
		{
			if (enemy.navMeshAgent.hasPath && enemy.navMeshAgent.remainingDistance < enemy.waypointAccuracy)
			{
				enemy.patrollingRoute.Next();

				if (UnityEngine.Random.value <= enemy.idleChance)
					enemy.SetState (enemy.patrolePause);
			}

			enemy.navMeshAgent.SetDestination (enemy.patrollingRoute.GetCurrentWaypoint());
		}
	}
}

public class PatrolePauseState : EnemyState
{
	float timer;

	public void Enter (EnemyStateMachine enemy)
	{
		timer = 0f;
		enemy.navMeshAgent.ResetPath();
	}

	public void Update (EnemyStateMachine enemy)
	{
		if (enemy.playerInSight)
			enemy.SetState (EnemyStateMachine.approach);
		else
		{
			if (timer >= enemy.idleDuration)
				enemy.SetState (EnemyStateMachine.patrole);
			else
				timer += Time.deltaTime;
		}
	}
}

public class TakeDamageState : EnemyState
{
	public void Enter (EnemyStateMachine enemy) {}

	public void Update (EnemyStateMachine enemy)
	{
		enemy.animator?.SetTrigger("TakeDamage");

		if (enemy.currentHealth <= 0f)
			enemy.SetState (EnemyStateMachine.dying);
		else
			enemy.SetState (EnemyStateMachine.idle);
	}
}

public class DyingState : EnemyState
{
	public void Enter (EnemyStateMachine enemy) {}

	public void Update (EnemyStateMachine enemy)
	{
		enemy.navMeshAgent.isStopped = true;
		enemy.animator?.SetTrigger("Die");
		PlayerStats.Me().enemiesKilled++;
		enemy.SetState (enemy.dead);
	}
}

public class DeadState : EnemyState
{
	float timer = 0f;
	public void Enter (EnemyStateMachine enemy) {timer = 0f;}

	public void Update (EnemyStateMachine enemy)
	{
		if (timer <= enemy.despawnDelay)
			timer += Time.deltaTime;
		else
			enemy.gameObject.SetActive(false);
	}
}

public class ApproachState : EnemyState
{
	public void Enter (EnemyStateMachine enemy) {}

	public void Update (EnemyStateMachine enemy)
	{
		if (!enemy.playerInSight)
			enemy.SetState (new IdleState());
		else
		{
			float distance = Vector3.Distance (enemy.transform.position, Camera.main.transform.position);

			if (distance <= enemy.attackRange)
				enemy.SetState (enemy.attack);

			enemy.navMeshAgent.SetDestination (Camera.main.transform.position);
		}
	}
}

public class AttackState : EnemyState
{
	float attackDelayTimer;

	public void Enter (EnemyStateMachine enemy) {attackDelayTimer = 0f;}

	public void Update (EnemyStateMachine enemy)
	{
		if (enemy.navMeshAgent.hasPath)
			enemy.navMeshAgent.ResetPath();

		if (!enemy.playerInSight)
			enemy.SetState (EnemyStateMachine.idle);
		else
		{
			float distance = Vector3.Distance (enemy.transform.position, Camera.main.transform.position);

			if (distance > enemy.attackRange)
				enemy.SetState (EnemyStateMachine.approach);
			else
			{
				attackDelayTimer -= Time.deltaTime;

				if (attackDelayTimer <= 0f)
				{
					enemy.animator?.SetTrigger("Attack");
					attackDelayTimer = enemy.attackDelay;
				}
			}
		}
	}
}

public class EnemyStateMachine : MonoBehaviour, Damageable
{
	public bool playerInSight;
	public Animator animator;

	[Header ("Attack Settings")]
	public float attackRange;
	public float attackDelay = 2f;

	[Header ("Spotting Setting")]
	[SerializeField] Transform rayOrigin;
	[SerializeField] float rayRange = 25f;
	
	[Header ("AI Settings")]
	public NavMeshAgent navMeshAgent;
	public PatrollingRoute patrollingRoute;
	public float waypointAccuracy = 0.25f;
	public float idleDuration = 1f;
	public float idleChance = 0.25f;
	public float despawnDelay = 2f;

	public EnemyState currentState;
	public static IdleState idle = new IdleState();
	public static DyingState dying = new DyingState();
	public static ApproachState approach = new ApproachState();
	public static PatroleState patrole = new PatroleState();
	public static TakeDamageState takeDamage = new TakeDamageState();
	
	//Non static
	public DeadState dead = new DeadState();
	public PatrolePauseState patrolePause = new PatrolePauseState();
	public AttackState attack = new AttackState();

	public float maxHealth;
	public float currentHealth;

	public void SetState (EnemyState state)
	{
		currentState = state;
		state.Enter(this);
	}

	public void ApplyDamage (float dmg)
	{
		if (currentHealth > 0)
		{
			currentHealth -= dmg;
			Debug.Log("AUTSCH!!! DMG: " + dmg.ToString("0.00"));

			SetState (new TakeDamageState());
		}
	}
    // Start is called before the first frame update
    void Start()
    {
		currentState = new IdleState();
		currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = IsPlayerInSight();
		currentState.Update (this);
    }


	bool IsPlayerInSight()
	{
		bool inRange = false;

		Ray ray = new Ray(rayOrigin.position, (Camera.main.transform.position - rayOrigin.position).normalized);
		
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, rayRange))
			inRange = hit.collider.CompareTag ("Player");

		Debug.DrawRay (ray.origin, ray.direction * rayRange, inRange ? Color.green : Color.red);

		return inRange;
	}
}
