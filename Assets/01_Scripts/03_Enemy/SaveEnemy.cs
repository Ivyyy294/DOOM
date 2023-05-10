using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveEnemy : SaveableObject
{
	[SerializeField] EnemyStateMachine enemy;

	public override string GetSerializedData()
	{
		return transform.position.x + ";" + transform.position.y + ";" + transform.position.z
			+ ";" + enemy.currentHealth;
	}

	public override void LoadObject(string data)
	{
		string[] list = data.Split(';');

		transform.position = new Vector3(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2]));
		enemy.currentHealth = float.Parse(list[3]);

		if (enemy.currentHealth <= 0)
			enemy.SetState(EnemyStateMachine.dying);
		else
			enemy.SetState(EnemyStateMachine.reset);
	}
}
