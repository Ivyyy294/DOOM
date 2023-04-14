using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Alarm : MonoBehaviour, InteractableObject
{
	[SerializeField] float speed = 0.5f;
	[SerializeField] float alarmDuration;
	[SerializeField] UnityEvent OnDeactivate;

	[Header ("Lara values")]
	[SerializeField] Material mOn;
	[SerializeField] Material mOff;
	[SerializeField] MeshRenderer meshRenderer;
	
	AudioSource audioSource;
	bool active = false;
	bool on = false;
	float timer = 0f;
	float alarmTimer = 0f;

	public void Interact()
	{
		if (!active)
		{
			audioSource?.Play();
			active = true;
			on = true;
		}
		alarmTimer = 0f;
	}

	void Start()
	{
		audioSource = GetComponent <AudioSource>();
	}
    // Update is called once per frame
    void Update()
    {
        if (meshRenderer != null)
		{
			if (!active)
				meshRenderer.material = mOff;
			else
				Blink();
		}

		if (active)
		{
			if (alarmTimer >= alarmDuration)
			{
				active = false;
				OnDeactivate?.Invoke();
				audioSource?.Stop();
			}

			alarmTimer += Time.deltaTime;
		}
    }

	void Blink()
	{
		timer += Time.deltaTime;

		if (timer >= speed)
		{
			on = !on;
			timer = 0f;
		}

		meshRenderer.material = on ? mOn : mOff;
	}
}
