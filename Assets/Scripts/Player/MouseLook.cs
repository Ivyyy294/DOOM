using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	Transform cameraTrans;
	float verticalRotation;
	[SerializeField] float maxVerticalAngle = 80f;

	[Range (0.1f, 2f)]
	[SerializeField] float mouseSensitivity = 1f;

    // Start is called before the first frame update
    void Start()
    {
		//Cursor.lockState = CursorLockMode.Locked;
		cameraTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
		MoveHorizontal();
		MoveVertical();
    }

	void MoveHorizontal() 
	{
		float mouseX = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (Vector3.up, mouseX);
	}

	void MoveVertical()
	{
		verticalRotation += Input.GetAxis ("Mouse Y")  * mouseSensitivity;
		verticalRotation = Mathf.Clamp (verticalRotation, -maxVerticalAngle, maxVerticalAngle);

		cameraTrans.localRotation = Quaternion.Euler (new Vector3 (-verticalRotation, 0f, 0f));
		//cameraTrans.Rotate (Vector3.right, -mouseY, Space.Self);
	}
}
