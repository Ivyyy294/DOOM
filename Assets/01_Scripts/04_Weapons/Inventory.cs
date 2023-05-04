using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
	[System.Serializable]
	public struct AmmoContainer
	{
		public Weapon.AmmoTyp ammoTyp;
		
		public int count;
		public int maxCount;
		public TextMeshProUGUI txtAmmo;
		public TextMeshProUGUI txtMaxAmmo;
	}

	private int armor;
	private int health;
	[SerializeField] TextMeshProUGUI txtArmor;
	[SerializeField] AmmoContainer[] ammoContainers;

    // Start is called before the first frame update
    void Start()
    {
        foreach (AmmoContainer i in ammoContainers)
		{
			if (i.txtMaxAmmo != null)
				i.txtMaxAmmo.text = i.maxCount.ToString();

			if (txtArmor != null)
				txtArmor.text = armor.ToString() + "%";
		}
    }

    // Update is called once per frame
    void Update()
    {
		foreach (AmmoContainer i in ammoContainers)
		{
			if (i.txtAmmo != null)
				i.txtAmmo.text = i.count.ToString();
		}
    }

	public int GetAmmoForReloading (Weapon.AmmoTyp ammoTyp, int count)
	{
		for (int i = 0; i < ammoContainers.Length; ++i)
		{
			if (ammoContainers[i].ammoTyp == ammoTyp)
			{
				int available = Mathf.Min (ammoContainers[i].count, count);
				ammoContainers[i].count -= available;
				return available;
			}
		}

		return 0;
	}

	public void AddAmmo (Weapon.AmmoTyp ammoTyp, int count)
	{
		for (int i = 0; i < ammoContainers.Length; ++i)
		{
			if (ammoContainers[i].ammoTyp == ammoTyp)
			{
				ammoContainers[i].count = Mathf.Min (ammoContainers[i].maxCount, ammoContainers[i].count + count);
				return;
			}
		}
	}

	public void AddArmor (int val)
	{
		armor = Mathf.Min (100, armor + val);
		txtArmor.text = armor.ToString()  + "%";
	}
}
