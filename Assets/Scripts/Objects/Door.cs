using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField] GameObject doorObject;
	[SerializeField] bool isOpen = false;

	[Range (1, 8)]
	[SerializeField] int switchNeededCount = 1;

	//Private Values
	int switchInputCount = 0;

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
			doorObject.SetActive (!open);
			isOpen = open;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
