using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
	[SerializeField] float dmgPerSecond;

	private void OnTriggerEnter(Collider other)
	{
		Damageable obj = other.gameObject.GetComponent<Damageable>();
		obj?.ApplyDamage (dmgPerSecond * Time.deltaTime);
	}
}
