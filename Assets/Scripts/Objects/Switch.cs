using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
	[SerializeField] bool isLocked = false;
	[SerializeField] UnityEvent onToggled;
	[SerializeField] UnityEvent onUntoggled;

	[Header ("Lara Values")]
	[SerializeField] Material mToggled;
	[SerializeField] Material mUntoggled;
	[SerializeField] Material mLocked;

	bool isToggled = false;
	private MeshRenderer meshRenderer;

	//Public
	public void Lock () { SetLockState (true);}
	public void Unlock () { SetLockState (false);}

	//Private
	void OnMouseDown()
	{
		if (!isLocked)
			SetActiveState (!isToggled);
	}

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
	}

	// Start is called before the first frame update
	void Start()
    {
        meshRenderer = GetComponent <MeshRenderer>();
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
