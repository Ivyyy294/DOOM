using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSwitch : SaveableObject
{
	[SerializeField] Switch obj;

	override public Payload GetPayload ()
	{
		Payload p = new Payload (uniqueId);
		p.Add ("toggled", obj.IsToggled());
		p.Add ("locked", obj.IsLocked());

		return p;
	}

	override public void LoadObject (Payload val)
	{
		bool locked = bool.Parse(val.data["locked"]);
		bool toggled = bool.Parse(val.data["toggled"]);

		if (locked)
			obj.Lock();
		else
			obj.Unlock();

		if (obj.IsToggled() != toggled)
			obj.Interact();
	}
}
