using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour, InteractableObject
{
	[SerializeField] Material mOn;
	[SerializeField] Material mOff;
	[SerializeField] MeshRenderer meshRenderer;
	[SerializeField] float speed = 0.5f;
	
	AudioSource audioSource;
	bool active = false;
	bool on = false;
	float timer = 0f;

	public void Interact()
	{
		if (!active)
		{
			audioSource?.Play();
			active = true;
			on = true;
		}
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
