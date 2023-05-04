using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shells : Collectable
{
	public int value;
	public Weapon.AmmoTyp typ;

   public override void Action (Inventory inventory)
	{
		if (inventory != null)
			inventory.AddAmmo (typ, value);

		gameObject.SetActive (false);
	}
}
