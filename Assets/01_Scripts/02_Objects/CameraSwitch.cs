using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
	[SerializeField] Transform rayOrigin;
	[SerializeField] float range;
	[SerializeField] float speed;
	[SerializeField] float maxAngleLeft;
	[SerializeField] float maxAngleRight;

	private float currentRotation = 0f;
	bool moveBack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (!moveBack)
			MoveRight();
		else
			MoveLeft();

        if (Search())
			Debug.Log("Player detected!");
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

	bool Search ()
	{
		Ray ray = new Ray (rayOrigin.position, rayOrigin.forward);

		RaycastHit hit;

		bool inRange = false;

		if (Physics.Raycast(ray, out hit, range))
			inRange = hit.collider.CompareTag ("Player");

		Debug.DrawRay (ray.origin, ray.direction * range, inRange ? Color.green : Color.red);

		return inRange;
	}
}
