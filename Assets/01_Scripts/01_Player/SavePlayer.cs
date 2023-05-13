using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer : SaveableObject
{
	[SerializeField] PlayerMovement3D player;
	[SerializeField] MouseLook mouseLook;
	[SerializeField] WeaponManager weaponManager;
	[SerializeField] Inventory inventory;

	public override Payload GetPayload()
	{
		Payload p = new Payload(uniqueId);
		//Position
		p.Add ("posX", transform.position.x);
		p.Add ("posY", transform.position.y);
		p.Add ("posZ", transform.position.z);

		//Rotation
		p.Add ("rotX", mouseLook.GetRotationX());
		p.Add ("rotY", mouseLook.GetRotationY());

		//WeaponManager
		p.Add ("currentWeapon", weaponManager.currentWeaponIndex);
		for (int i = 0; i < weaponManager.weaponContainers.Count; ++i)
			p.Add ("w" + i, weaponManager.weaponContainers[i].currentAmmo.ToString());

		//Player stats
		p.Add ("keyFound", PlayerStats.Me().keysFound);
		p.Add ("shotsFired", PlayerStats.Me().shotsFired);
		p.Add ("hits", PlayerStats.Me().hits);
		p.Add ("enemiesKilled", PlayerStats.Me().enemiesKilled);
		p.Add ("deathCounts", PlayerStats.Me().deathCounts);
		p.Add ("secretsFound", PlayerStats.Me().secretsFound);
		p.Add ("items", PlayerStats.Me().items);

		//Inventory
		p.Add ("hp", inventory.health);
		p.Add ("armor", inventory.armor);

		return p;
	}

	public override void LoadObject(Payload val)
	{
		//Position
		Vector3 loadedPos = new Vector3();
		loadedPos.x = float.Parse(val.data["posX"]);
		loadedPos.y = float.Parse(val.data["posY"]);
		loadedPos.z = float.Parse(val.data["posZ"]);
		player.SetPosition(loadedPos);

		//Rotation
		mouseLook.SetRotationX (float.Parse(val.data["rotX"]));
		mouseLook.SetRotationY (float.Parse(val.data["rotY"]));
		
		//WeaponManager
		for (int i = 0; i < weaponManager.weaponContainers.Count; ++i)
			weaponManager.weaponContainers[i].currentAmmo = int.Parse(val.data["w" + i]);

		weaponManager.SetCurrentWeaponIndex (int.Parse(val.data["currentWeapon"]));

		//Player stats
		PlayerStats.Me().Reset();
		PlayerStats.Me().keysFound = int.Parse (val.data["keyFound"]);
		PlayerStats.Me().shotsFired = int.Parse (val.data["shotsFired"]);
		PlayerStats.Me().hits = int.Parse (val.data["hits"]);
		PlayerStats.Me().enemiesKilled = int.Parse (val.data["enemiesKilled"]);
		PlayerStats.Me().deathCounts = int.Parse (val.data["deathCounts"]);
		PlayerStats.Me().secretsFound = int.Parse (val.data["secretsFound"]);
		PlayerStats.Me().items = int.Parse (val.data["items"]);

		inventory.health = float.Parse (val.data["hp"]);
		inventory.armor = int.Parse (val.data["armor"]);
	}
}

