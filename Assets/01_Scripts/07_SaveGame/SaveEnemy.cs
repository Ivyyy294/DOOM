using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ivyyy.SaveGameSystem;

public class SaveEnemy : SaveableObject
{
	[SerializeField] EnemyStateMachine enemy;

	public override Payload GetPayload()
	{
		Payload p = new Payload(UniqueId);

		p.Add ("posX", transform.position.x);
		p.Add ("posY", transform.position.y);
		p.Add ("posZ", transform.position.z);

		p.Add ("hp", enemy.currentHealth);

		return p;
	}

	public override void LoadObject(Payload data)
	{
		transform.position = new Vector3(float.Parse(data.data["posX"]), float.Parse(data.data["posY"]), float.Parse(data.data["posZ"]));
		enemy.currentHealth = float.Parse(data.data["hp"]);

		if (enemy.currentHealth <= 0)
		{
			enemy.animator.SetTrigger ("Dead");
			enemy.AddState(EnemyStateMachine.dead);
		}
		else
			enemy.AddState(EnemyStateMachine.reset);
	}
}
