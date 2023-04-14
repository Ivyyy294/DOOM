using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSwitch : MonoBehaviour, InteractableObject
{
	[SerializeField] Light lightRef;
	[SerializeField] Transform rayOrigin;
	[SerializeField] float idleSpeed;
	[SerializeField] float trackingSpeed;
	[SerializeField] float maxAngleLeft;
	[SerializeField] float maxAngleRight;
	[SerializeField] UnityEvent OnInteract;
	[SerializeField] bool moveBack = false;

	private float currentRotation = 0f;
	float range;
	float size;
	bool tracking = false;

	public void Interact()
	{
		if (tracking)
			tracking = false;
	}

	private void Start()
	{
		if (lightRef != null)
		{
			range = lightRef.range;
			size = lightRef.spotAngle;
		}
	}

	// Update is called once per frame
	void Update()
    {
		if (!tracking)
			Idle();
		else
			Tracking();
    }

	void Tracking()
	{
		Vector3 targetDir = Camera.main.transform.position - transform.position;
		Vector3 forward = transform.forward;
		float angle = Vector3.SignedAngle (targetDir, forward, Vector3.up);

		if (angle > 0f)
			MoveLeft(trackingSpeed);
		else if (angle < 0f)
			MoveRight(trackingSpeed);

		if (Search(Camera.main.transform.position - rayOrigin.position))
			OnInteract.Invoke();

	}

	void Idle()
	{
		if (!moveBack)
			MoveRight(idleSpeed);
		else
			MoveLeft(idleSpeed);

        if (Search(rayOrigin.forward))
		{
			OnInteract?.Invoke();
			tracking = true;
		}
	}

	void MoveRight(float speed)
	{
		float rotVal = Mathf.Min (speed * Time.deltaTime, maxAngleRight - currentRotation);
		currentRotation += rotVal;
		transform.Rotate (Vector3.up, rotVal);

		if (currentRotation == maxAngleRight)
			moveBack = true;
	}

	void MoveLeft(float speed)
	{
		float rotVal = Mathf.Min (speed * Time.deltaTime, maxAngleLeft + currentRotation);
		currentRotation -= rotVal;
		transform.Rotate (Vector3.up, -rotVal);

		if (currentRotation == -maxAngleLeft)
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
