using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchKey : MonoBehaviour
{
	[SerializeField] UnityEvent OnCollected;

	private void OnTriggerEnter(Collider other)
	{
		if (OnCollected != null)
			OnCollected.Invoke();

		Destroy (gameObject);
	}
}
