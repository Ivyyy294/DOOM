using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCollectable : SaveableObject
{
	[SerializeField] GameObject obj;
	override public Payload GetPayload ()
	{
		Payload p = new Payload(UniqueId);
		
		if (obj != null)
			p.Add ("active", obj.activeInHierarchy);
		
		return p;
	}

	override public void LoadObject (Payload data)
	{
		bool active = bool.Parse (data.data["active"]);
		obj?.SetActive (active);
	}
}
