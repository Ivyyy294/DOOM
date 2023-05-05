using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class Secret: MonoBehaviour
{
	[SerializeField] Sprite sprite;

	private void Start()
	{
		GetComponent <SpriteRenderer>().sprite = sprite;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player"))
		{
			PlayerStats.Me().secretsFound++;
			gameObject.SetActive (false);
		}
	}
}
