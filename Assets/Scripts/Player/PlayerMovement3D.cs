using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
	[Header("Running")]
	[SerializeField] float baseSpeed;
	[SerializeField] float sprintSpeed;
	CharacterController characterController;

	[Header("Jumping")]
	[SerializeField] float gravity = 1f;
	[SerializeField] float jumpHeight = 1f;
	[SerializeField] int jumpNumber = 2;

	[SerializeField] List <AudioClip> aJump;

	Vector3 verticalMovement;

	//private Values
	private float currentSpeed;
	private int jumpCounter = 0;
	private AudioSource audioSource;
	
	//Timer

	//[SerializeField] AnimationCurve accelerationCurve;
	//[SerializeField] AnimationCurve deaccelerationCurve;
	//[SerializeField] AnimationCurve jumpCurve;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent <CharacterController>();
		audioSource = GetComponent <AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		characterController.Move (GetHorizontalMovement() + (GetVerticalMovement() * Time.deltaTime));
    }

	Vector3 GetHorizontalMovement()
	{
		bool isSprinting = Input.GetKey (KeyCode.LeftShift);
		currentSpeed = ((isSprinting) ? sprintSpeed : baseSpeed) * Time.deltaTime;

		//Camera.main.fieldOfView = isSprinting ? 70f : 60f;

        float xInput = Input.GetAxis ("Horizontal");
		float yInput = Input.GetAxis ("Vertical");

		Vector3 movementVec = transform.right * xInput + transform.forward * yInput;

		if (movementVec.magnitude > 1f)
			movementVec = movementVec.normalized;

		movementVec *= currentSpeed;

		return movementVec;
	}

	Vector3 GetVerticalMovement()
	{
		//Gravity
		if (characterController.isGrounded)
		{
			jumpCounter = 0;
			verticalMovement.y = -2f;
		}
		else
			verticalMovement.y -= gravity * Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Space) && (characterController.isGrounded || jumpCounter < jumpNumber))
		{
			verticalMovement.y = Mathf.Sqrt(jumpHeight * gravity * 2f);
			jumpCounter++;

			if (aJump != null && aJump.Count > 0)
			{
				int index = Random.Range (0, aJump.Count);
				audioSource?.PlayOneShot (aJump[index]);
			}
		}

		return verticalMovement;
	}
}
