using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField] Transform openPos;
	[SerializeField] float openSpeed;
	[SerializeField] GameObject doorObject;
	[SerializeField] bool isOpen = false;

	[Range (1, 8)]
	[SerializeField] int switchNeededCount = 1;

	//Private Values
	int switchInputCount = 0;
	float timer;

	//public functions
	public void OpenDoor()
	{
		++switchInputCount;

		if (switchInputCount >= switchNeededCount)
			ChangeState (true);
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
			timer = 0f;
		}
	}

	private void Move (Vector3 startPos, Vector3 destPos)
	{
		float distance = Vector3.Distance (startPos, destPos);

		float distCovered = timer * openSpeed;

		float FractionOfJourney = distCovered / distance;

		doorObject.transform.localPosition = Vector3.Lerp(startPos, destPos, FractionOfJourney);

		timer += Time.fixedDeltaTime;
	}

	void FixedUpdate()
	{
		if (isOpen && doorObject.transform.localPosition != openPos.localPosition)
			Move (Vector3.zero, openPos.localPosition);
		else if (!isOpen && doorObject.transform.localPosition != Vector3.zero)
			Move (openPos.localPosition, Vector3.zero);
	}
}
