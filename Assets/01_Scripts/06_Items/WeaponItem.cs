using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : Collectable
{
	[SerializeField] Weapon weapon;

	public override void Action (Inventory inventory)
	{
		WeaponManager weaponManager = inventory.gameObject.GetComponent<WeaponManager>();

		weaponManager?.UnlockWeapon (weapon);

		gameObject.SetActive (false);
	}
}
