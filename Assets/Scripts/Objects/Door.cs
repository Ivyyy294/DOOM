using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[Range (1, 8)]
	[SerializeField] int switchNeededCount = 1;
	[SerializeField] float openSpeed;
	[SerializeField] float autoClose = 0f;

	[Header ("Lara Values")]
	[SerializeField] Transform openPos;
	[SerializeField] GameObject doorObject;
	[SerializeField] bool isOpen = false;

	//Private Values
	int switchInputCount = 0;
	float moveTimer;
	float autoCloseTimer = 0f;

	//public functions
	public void OpenDoor()
	{
		++switchInputCount;

		if (switchInputCount >= switchNeededCount)
		{
			autoCloseTimer = 0f;
			ChangeState (true);
		}
	}

	public void CloseDoor()
	{
		if (switchInputCount > 0)
			--switchInputCount;

		if (switchInputCount < switchNeededCount)
			ChangeState (false);
	}

	//Private functions
	void ChangeState (bool open)
	{
		if (isOpen != open && doorObject != null)
		{
			isOpen = open;
			moveTimer = 0f;
		}
	}

	private void Move (Vector3 startPos, Vector3 destPos)
	{
		float distance = Vector3.Distance (startPos, destPos);

		float distCovered = moveTimer * openSpeed;

		float FractionOfJourney = distCovered / distance;

		doorObject.transform.localPosition = Vector3.Lerp(startPos, destPos, FractionOfJourney);

		moveTimer += Time.fixedDeltaTime;
	}

	void FixedUpdate()
	{
		if (isOpen && doorObject.transform.localPosition != openPos.localPosition)
			Move (Vector3.zero, openPos.localPosition);
		else if (!isOpen && doorObject.transform.localPosition != Vector3.zero)
			Move (openPos.localPosition, Vector3.zero);

		if (isOpen && autoClose > 0f)
		{
			if (autoCloseTimer < autoClose)
				autoCloseTimer += Time.fixedDeltaTime;
			else
				CloseDoor();
		}
	}
}
