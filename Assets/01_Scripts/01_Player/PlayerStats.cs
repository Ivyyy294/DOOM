using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	private static PlayerStats me;
	public static PlayerStats Me() { return me;}

	public int keysFound;
	public int bullets;
	public int enemiesKilled;
	public int deathCounts;
	public int secretsFound;

    // Start is called before the first frame update
    void Start()
    {
        if (me == null)
			me = this;
    }
}
