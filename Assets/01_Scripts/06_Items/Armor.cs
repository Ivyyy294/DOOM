using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Collectable
{
	public int value;

   public override void Action (Inventory inventory)
	{
		if (inventory != null)
			inventory.AddArmor (value);

		gameObject.SetActive (false);
	}
}
