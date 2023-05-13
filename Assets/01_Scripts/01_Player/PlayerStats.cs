using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	private static PlayerStats me;
	public static PlayerStats Me() { return me;}

	public int keysFound;
	public int shotsFired;
	public int hits;
	public int enemiesKilled;
	public int deathCounts;
	public int secretsFound;
	public int items;

	//Dynamic values
	public int maxEnemy;
	public int maxKeys;
	public int maxSecrets;
	public int maxItems;

	public bool loadGameOnStart = false;

	public void Init()
	{
		maxEnemy = FindObjectsOfType <EnemyStateMachine>().Length;
		maxKeys = FindObjectsOfType <SwitchKey>().Length;
		maxItems = FindObjectsOfType <Collectable>().Length;
		maxSecrets = FindObjectsOfType <Secret>().Length;
	}

	public void Reset()
	{
		keysFound = 0;
		shotsFired = 0;
		hits = 0;
		enemiesKilled = 0;
		deathCounts = 0;
		secretsFound = 0;
		items = 0;
		loadGameOnStart = false;
	}

	// Start is called before the first frame update
	void Start()
    {
        if (me == null)
		{
			me = this;
			DontDestroyOnLoad (gameObject);
		}
		else
			Destroy (gameObject);
    }
}
