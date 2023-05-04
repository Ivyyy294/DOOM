using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag ("Player"))
		{
			Inventory inventory = other.gameObject.GetComponent <Inventory>();
			Action (inventory);
		}
	}

	public virtual void Action (Inventory inventory){ }
}
