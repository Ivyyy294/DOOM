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
	Vector3 verticalMovement;

	//private Values
	private float currentSpeed;
	private bool doubleJumpPossible = true;
	//Timer

	//[SerializeField] AnimationCurve accelerationCurve;
	//[SerializeField] AnimationCurve deaccelerationCurve;
	//[SerializeField] AnimationCurve jumpCurve;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent <CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		characterController.Move (GetHorizontalMovement() + (GetVerticalMovement() * Time.deltaTime));
    }

	Vector3 GetHorizontalMovement()
	{
		currentSpeed = (Input.GetKey (KeyCode.LeftShift) ? sprintSpeed : baseSpeed) * Time.deltaTime;

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
		if (characterController.isGrounded)
		{
			if (!doubleJumpPossible)
			doubleJumpPossible = true;
			verticalMovement.y = -2f;
			
			if (Input.GetKey (KeyCode.Space))
				verticalMovement.y = Mathf.Sqrt (jumpHeight * gravity * 2f);
		}
		else
		{
		
			verticalMovement.y -= gravity * Time.deltaTime;
		}

		return verticalMovement;
	}
}
