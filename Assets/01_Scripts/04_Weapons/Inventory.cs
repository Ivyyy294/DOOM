using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour, Damageable
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
	private float health;
	[SerializeField] TextMeshProUGUI txtArmor;
	[SerializeField] TextMeshProUGUI txtHealth;
	[SerializeField] AmmoContainer[] ammoContainers;

	public void ApplyDamage (float dmg)
	{
		float dmgReduction = 0.5f * ((float)armor / 100f);

		health -= dmg * (1 - dmgReduction);
		txtHealth.text = health.ToString("0") + "%";
	}

    // Start is called before the first frame update
    void Start()
    {
		health = 100;

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
