using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField] float range;
	[SerializeField] KeyCode interactKey;
	[SerializeField] WeaponManager weaponManager;
	[SerializeField] PauseMenu pauseMenu;

	private Transform cameraTrans;

	//Private
	private void Start()
	{
		cameraTrans = Camera.main.transform;
		weaponManager = GetComponent <WeaponManager>();
	}

	// Update is called once per frame
	void Update()
    {
		if (Time.timeScale > 0f)
		{
			if (Input.GetKeyDown (interactKey))
			{
				//Shoot if no interaction is possible
				if (!Interact())
					weaponManager?.Shoot();
			}

			if (Input.GetKeyDown (KeyCode.Escape))
				pauseMenu.gameObject.SetActive (true);
		}
    }

	bool Interact()
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

		return inRange;
	}
}
