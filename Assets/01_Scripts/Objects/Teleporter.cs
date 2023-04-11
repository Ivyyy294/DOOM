using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
	[SerializeField] Transform targetPos;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player"))
			other.transform.position = targetPos.position;
	}
}
