using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour
{
	[SerializeField] float speed;

	[SerializeField] UnityEvent onActive;
	[SerializeField] UnityEvent onInactive;

	[Header ("Lara Values")]
	[SerializeField] Transform platform;
	[SerializeField] Transform startPos;
	[SerializeField] Transform destPos;

	bool active = false;
	Rigidbody myRigidbody;

	public void Start()
	{
		myRigidbody = GetComponent <Rigidbody>();
		transform.position = startPos.position;
	}

	public void Activate()
	{
		if (!active)
		{
			active = true;
			onActive?.Invoke();
		}
	}

	public void Deactivate()
	{
		if (active)
		{
			active = false;
			onInactive?.Invoke();
		}
	}

	void Move (Vector3 pos)
	{
		float minDis = Mathf.Min (speed * Time.fixedDeltaTime, Vector3.Distance (platform.position, pos));
		Vector3 movementVec = (pos - platform.position).normalized * minDis;

		myRigidbody?.MovePosition (platform.position + movementVec);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Vector3 tmpPos = active ? destPos.position : startPos.position;

		if (platform.position != tmpPos)
			Move (tmpPos);
    }

	private void OnTriggerEnter(Collider other)
	{
		other.transform.SetParent (transform);
	}

	private void OnTriggerExit(Collider other)
	{
		other.transform.SetParent (null);
	}
}
