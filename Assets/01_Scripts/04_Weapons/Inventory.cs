using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	public int armor;
	public float health = 100;
	[SerializeField] TextMeshProUGUI txtArmor;
	[SerializeField] TextMeshProUGUI txtHealth;
	[SerializeField] Image imgFace;
	[SerializeField] Sprite[] faces;
	[SerializeField] GameObject uiDead;

	public AmmoContainer[] ammoContainers;

	public void ApplyDamage (float dmg)
	{
		float dmgReduction = 0.5f * ((float)armor / 100f);

		health -= dmg * (1 - dmgReduction);

		if (health <= 0f)
			uiDead?.SetActive (true);
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

		float tmp = 100f / faces.Length;

		for (int i = 0; i < faces.Length; ++i)
		{
			if (health > 100f - tmp * (i + 1))
			{
				imgFace.sprite = faces[i];
				break;
			}
		}

		txtHealth.text = health.ToString("0") + "%";
		txtArmor.text = armor.ToString()  + "%";
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
	}
}
