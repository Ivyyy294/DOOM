using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraSwitch : MonoBehaviour
{
	[SerializeField] Transform rayOrigin;
	[SerializeField] float range;
	[SerializeField] float speed;
	[SerializeField] float maxAngleLeft;
	[SerializeField] float maxAngleRight;
	[SerializeField] UnityEvent OnInteract;

	private float currentRotation = 0f;
	bool moveBack = false;
	bool tracking = false;

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
		//tracking = false;
		//rotVal = Mathf.Clamp (rotVal, -maxAngleLeft, maxAngleRight);
		//rotVal = currentRotation - rotVal;		
		//transform.Rotate (Vector3.up, rotVal);
		//tracking = Search(Camera.main.transform.position - rayOrigin.position);
	}

	void Idle()
	{
		if (!moveBack)
			MoveRight();
		else
			MoveLeft();

        if (Search(rayOrigin.forward))
		{
			OnInteract?.Invoke();
			tracking = true;
		}
	}

	void MoveRight()
	{
		float rotVal = Mathf.Min (speed * Time.deltaTime, maxAngleRight - currentRotation);
		currentRotation += rotVal;
		transform.Rotate (Vector3.up, rotVal);

		if (currentRotation == maxAngleRight)
			moveBack = true;
	}

	void MoveLeft()
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
