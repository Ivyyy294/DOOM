using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer : SaveableObject
{
	[SerializeField] PlayerMovement3D player;
	[SerializeField] MouseLook mouseLook;
	[SerializeField] WeaponManager weaponManager;

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

		return p;
	}

	public override void LoadObject(Payload val)
	{
		Vector3 loadedPos = new Vector3();
		loadedPos.x = float.Parse(val.data["posX"]);
		loadedPos.y = float.Parse(val.data["posY"]);
		loadedPos.z = float.Parse(val.data["posZ"]);
		player.SetPosition(loadedPos);

		mouseLook.SetRotationX (float.Parse(val.data["rotX"]));
		mouseLook.SetRotationY (float.Parse(val.data["rotY"]));
		
		for (int i = 0; i < weaponManager.weaponContainers.Count; ++i)
			weaponManager.weaponContainers[i].currentAmmo = int.Parse(val.data["w" + i]);

		weaponManager.SetCurrentWeaponIndex (int.Parse(val.data["currentWeapon"]));
	}
}
