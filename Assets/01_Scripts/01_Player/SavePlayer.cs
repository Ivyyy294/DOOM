using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayer : SaveableObject
{
	[SerializeField] PlayerMovement3D player;
	[SerializeField] MouseLook mouseLook;
	[SerializeField] WeaponManager weaponManager;

	public override string GetSerializedData()
	{
		return transform.position.x
			+ ";" + transform.position.y
			+ ";" + transform.position.z
			+ ";" + mouseLook.GetRotationX()
			+ ";" + mouseLook.GetRotationY()
			+ ";" + weaponManager.GetSerializedData();
	}

	public override void LoadObject(string val)
	{
		string[] data = val.Split(';');
		Vector3 loadedPos = new Vector3();
		loadedPos.x = float.Parse(data[0]);
		loadedPos.y = float.Parse(data[1]);
		loadedPos.z = float.Parse(data[2]);
		player.SetPosition(loadedPos);

		mouseLook.SetRotationX (float.Parse(data[3]));
		mouseLook.SetRotationY (float.Parse(data[4]));

		string[] tmp = new string [data.Length - 5];
		System.Array.Copy (data, 5, tmp, 0, tmp.Length);
		weaponManager.LoadObject (tmp);
	}
}
