using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchKey : MonoBehaviour
{
	[SerializeField] UnityEvent OnCollected;
	[SerializeField] AudioClip audioCollected;
	private AudioSource audioSource;
	float timer = 0f;
	bool collected;

	private void Start()
	{
		audioSource = GetComponent <AudioSource>();
	}

	private void OnEnable()
	{
		collected = false;
		timer = 0f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player") && !collected)
		{
			PlayerStats.Me().keysFound++;
			
			collected = true;

			if (OnCollected != null)
				OnCollected.Invoke();

			audioSource?.PlayOneShot (audioCollected);
		}
	}

	private void Update()
	{
		if (collected)
		{
			if (timer <= 0.5f)
				timer += Time.deltaTime;
			else
				gameObject.SetActive (false);
		}
	}
}
