using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour , InteractableObject
{
	[SerializeField] bool isLocked = false;
	[SerializeField] UnityEvent onToggled;
	[SerializeField] UnityEvent onUntoggled;

	[Header ("Sounds")]
	[SerializeField] AudioClip aToggle;
	[SerializeField] AudioClip aLocked;

	[Header ("Lara Values")]
	[SerializeField] Material mToggled;
	[SerializeField] Material mUntoggled;
	[SerializeField] Material mLocked;

	bool isToggled = false;
	private MeshRenderer meshRenderer;
	private AudioSource audioSource;

	//Public
	public bool IsLocked() {return isLocked;}
	public bool IsToggled() {return isToggled;}

	public void Lock () { SetLockState (true);}
	public void Unlock () { SetLockState (false);}
	
	public void OverrideState (bool val)
	{
		isToggled = val;
		SetMaterial();
	}

	public void Interact()
	{
		if (!isLocked)
		{
			SetActiveState (!isToggled);
			audioSource?.PlayOneShot (aToggle);
		}
		else
			audioSource?.PlayOneShot (aLocked);
	}

	//Private
	private void SetActiveState (bool val)
	{
		isToggled = val;

		if (isToggled)
			onToggled.Invoke();
		else
			onUntoggled.Invoke();
	
		SetMaterial();
	}



	private void SetLockState (bool val)
	{
		isLocked = val;
		SetMaterial();

		if (isLocked)
			SetActiveState (false);
	}

	// Start is called before the first frame update
	void Start()
    {
        meshRenderer = GetComponent <MeshRenderer>();
		audioSource = GetComponent <AudioSource>();

		SetMaterial();
    }

	void SetMaterial()
	{
		if (meshRenderer != null)
		{
			if (isLocked)
				meshRenderer.material = mLocked;
			else
				meshRenderer.material = isToggled ? mToggled : mUntoggled;
		}
	}
}
