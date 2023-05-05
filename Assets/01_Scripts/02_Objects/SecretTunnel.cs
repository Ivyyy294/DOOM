using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretTunnel : MonoBehaviour, InteractableObject
{
    public void Interact()
	{
		gameObject.SetActive (!gameObject.activeInHierarchy);
	}
}
