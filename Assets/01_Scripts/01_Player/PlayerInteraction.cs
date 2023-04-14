using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] float range;
	[SerializeField] KeyCode interactKey;

	private Transform cameraTrans;


	private void Start()
	{
		cameraTrans = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown (interactKey))
			Interact();
    }

	void Interact()
	{
		Ray ray = new Ray (cameraTrans.position, cameraTrans.forward);

		RaycastHit hit;

		bool inRange = false;

		if (Physics.Raycast(ray, out hit, range))
		{
			InteractableObject tmp = hit.transform.gameObject.GetComponent<InteractableObject>();
			
			if (tmp != null)
			{
				tmp.Interact();
				inRange = true;
			}
		}

		Debug.DrawRay (ray.origin, ray.direction * range, inRange ? Color.green : Color.red);
	}
}
