using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof (Rigidbody))]
public class TriggerZone : MonoBehaviour
{
	[SerializeField] UnityEvent onTriggerEnter;
	[SerializeField] UnityEvent onTriggerExit;

	private void OnTriggerEnter(Collider other)
	{
		if (onTriggerEnter != null && other.CompareTag ("Player"))
			onTriggerEnter.Invoke();
	}

	private void OnTriggerExit(Collider other)
	{
		if (onTriggerExit != null && other.CompareTag ("Player"))
			onTriggerExit.Invoke();
	}
}
