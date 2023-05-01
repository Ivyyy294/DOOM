using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] float maxMovement;
	[SerializeField] float frequenzy;
	[SerializeField] AnimationCurve animationCurve;

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
				cameraObject.transform.localPosition = startPos + (Vector3.up * animationCurve.Evaluate (timeRunning) * maxMovement);
				//cameraObject.transform.localPosition = startPos + (Vector3.up * Mathf.Sin (timeRunning) * maxMovement);
				timeRunning += Time.deltaTime * frequenzy;
			}
			else
			{
				timeRunning = 0f;
				currentState = State.RESET;
			}
		}
		//Finish current arc
		else if (currentState == State.RESET)
		{
			float distance = Vector3.Distance (startPos, cameraObject.transform.localPosition);

			if (distance > 0f)
			{
				//float speedNew = maxMovement * (1 / (animationCurve[animationCurve.length - 1].time) * 0.5f);
				float speedAlt = maxMovement * frequenzy;
				float stepSize = Mathf.Min (distance, speedAlt * Time.deltaTime);
				cameraObject.transform.localPosition = Vector3.MoveTowards (cameraObject.transform.localPosition, startPos, stepSize);
			}
			else
				currentState = State.IDLE;
		}
		else if (IsRunning())
			currentState = State.SHAKING;
	}

	bool IsRunning ()
	{
		return characterController.isGrounded && (Input.GetAxis ("Horizontal") != 0f || Input.GetAxis ("Vertical") != 0f);
	}
}
