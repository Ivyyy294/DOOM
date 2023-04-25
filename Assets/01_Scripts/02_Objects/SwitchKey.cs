using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchKey : MonoBehaviour
{
	[SerializeField] UnityEvent OnCollected;
	[SerializeField] AudioClip audioCollected;
	private AudioSource audioSource;
	private bool collected = false;

	private void Start()
	{
		audioSource = GetComponent <AudioSource>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!collected)
		{
			PlayerStats.Me().keysFound++;
			
			collected = true;

			if (OnCollected != null)
				OnCollected.Invoke();

			audioSource?.PlayOneShot (audioCollected);

			Destroy (gameObject, 0.5f);
		}
	}
}
