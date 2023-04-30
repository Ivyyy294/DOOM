using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] float maxMovement;
	[SerializeField] float frequenzy;
	[SerializeField] AnimationCurve test;

	enum State
	{
		SHAKING,
		RESET,
		IDLE
	}
	State currentState = State.IDLE;
    // Start is called before the first frame update
	private GameObject cameraObject;
	private Vector3 startPos;
	float timeRunning;
	CharacterController characterController;


    void Start()
    {
        cameraObject = Camera.main.gameObject;
		startPos = cameraObject.transform.localPosition;
		characterController = GetComponent <CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		if (currentState == State.SHAKING)
		{
			if (IsRunning())
			{
				cameraObject.transform.localPosition = startPos + (Vector3.up * Mathf.Sin(timeRunning * frequenzy) * maxMovement);
				timeRunning += Time.deltaTime;
			}
			else
			{
				timeRunning = 0f;
				currentState = State.RESET;
			}
		}
		else if (currentState == State.RESET)
		{
			float speed = Time.deltaTime * frequenzy * maxMovement;
			cameraObject.transform.localPosition = Vector3.MoveTowards (cameraObject.transform.localPosition, startPos, speed);

			if (cameraObject.transform.localPosition == startPos)
				currentState = State.IDLE;
		}
		else if (IsRunning())
			currentState = State.SHAKING;
	}

	bool IsRunning ()
	{
		return (characterController.isGrounded &&
				(Input.GetAxis ("Horizontal") != 0f
				|| Input.GetAxis ("Vertical") != 0f));
	}
}
