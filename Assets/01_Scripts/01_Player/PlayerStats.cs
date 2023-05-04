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

	public int maxEnemy;
	public int maxKeys;
	public int maxSecrets;
	public int maxItems;


    // Start is called before the first frame update
    void Start()
    {
        if (me == null)
		{
			me = this;
			DontDestroyOnLoad (gameObject);

			maxEnemy = FindObjectsOfType <EnemyStateMachine>().Length;
			maxKeys = FindObjectsOfType <SwitchKey>().Length;
			maxItems = FindObjectsOfType <Collectable>().Length;
		}
    }
}
