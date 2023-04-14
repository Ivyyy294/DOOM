using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSwitch : MonoBehaviour, InteractableObject
{
	[SerializeField] float speed;
	[SerializeField] float maxAngleLeft;
	[SerializeField] float maxAngleRight;
	[SerializeField] UnityEvent OnInteract;
	[SerializeField] bool moveBack = false;

	[Header ("Lara Values")]
	[SerializeField] Light lightRef;
	[SerializeField] Transform rayOrigin;

	enum State
	{
		IDLE,
		TRACKING,
		RESET
	}

	private Quaternion resetPos;
	float range;
	State currentState = State.IDLE;

	public void Interact()
	{
		if (currentState == State.TRACKING)
			currentState = State.RESET;
	}

	private void Start()
	{
		resetPos = transform.localRotation;

		if (lightRef != null)
			range = lightRef.range;
	}

	// Update is called once per frame
	void Update()
    {
		if (currentState == State.IDLE)
			Idle();
		else if (currentState == State.TRACKING)
			Tracking();
		else
			Reset();
    }

	private void Reset()
	{
		Rotate (resetPos);

		if (transform.localRotation == resetPos)
			currentState = State.IDLE;
	}

	void Rotate (Quaternion target)
	{
		transform.localRotation = Quaternion.RotateTowards (transform.localRotation, target, speed * Time.deltaTime);
	}

	void Tracking()
	{
		Vector3 targetDir = Camera.main.transform.position - transform.position;
		transform.forward = targetDir;

		if (Search(rayOrigin.forward))
			OnInteract.Invoke();
	}

	void Idle()
	{
		if (!moveBack)
			MoveRight(speed);
		else
			MoveLeft(speed);

        if (Search(rayOrigin.forward))
		{
			OnInteract?.Invoke();
			currentState = State.TRACKING;
		}
	}

	void MoveRight(float speed)
	{
		Quaternion tmp = Quaternion.Euler (transform.eulerAngles.x, maxAngleRight, 0f);
		Rotate (tmp);

		if (transform.localRotation == tmp)
			moveBack = true;
	}

	void MoveLeft(float speed)
	{
		Quaternion tmp = Quaternion.Euler (transform.eulerAngles.x, -maxAngleLeft, 0f);
		Rotate (tmp);

		if (transform.localRotation == tmp)
			moveBack = false;
	}

	bool Search (Vector3 direction)
	{
		Ray ray = new Ray (rayOrigin.position, direction);

		RaycastHit hit;

		bool inRange = false;

		if (Physics.Raycast(ray, out hit, range))
			inRange = hit.collider.CompareTag ("Player");

		Debug.DrawRay (ray.origin, ray.direction * range, inRange ? Color.green : Color.red);

		return inRange;
	}
}
