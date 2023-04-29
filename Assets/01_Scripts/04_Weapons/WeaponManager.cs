using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponContainer
{
	public Weapon weapon;
	public int currentAmmo;

	public WeaponContainer (Weapon w)
	{
		weapon = w;
		currentAmmo = weapon.clipSize;
	}
}

public class WeaponManager : MonoBehaviour
{
	[SerializeField] List <Weapon> weapons;
	[SerializeField] int sampleRate = 1;
	[SerializeField] float switchWeaonDelay;

	[Header ("Lara Values")]
	[SerializeField] Image weaponSprite;
	[SerializeField] TextMeshProUGUI txtAmmoCounter;

	enum State
	{
		IDLE,
		SHOOTING,
		RELOADING
	}

	private float animationSpeed;
	private State currentState;
	private List <WeaponContainer> weaponContainers;
	private WeaponContainer currentWeapon;
	private float reloadTimer;
	private float shootTimer;
	private float switchWeaponTimer;
	private int currentWeaponIndex;
	private AudioSource audioSource;
	private Inventory inventory;
	
	//Public

	public void Idle ()
	{
		if (currentState != State.IDLE)
		{
			currentState = State.IDLE;
			weaponSprite.sprite = currentWeapon.weapon.idlSprite;
		}
		else
		{
			//Check Switch weapon
			SwitchWeapon();

			if (Input.GetMouseButtonDown (1))
				Reload();
		}
	}

	public void Reload()
	{
		if (currentState == State.IDLE)
		{
			if (inventory != null)
			{
				int available = inventory.GetAmmoForReloading (currentWeapon.weapon.ammoTyp, currentWeapon.weapon.clipSize);
				currentWeapon.currentAmmo = available;
			}
			else
				currentWeapon.currentAmmo = currentWeapon.weapon.clipSize;
			
			txtAmmoCounter.text = currentWeapon.currentAmmo.ToString();

			if (currentWeapon.currentAmmo > 0)
			{
				currentState = State.RELOADING;
				reloadTimer = 0f;
				audioSource?.PlayOneShot (currentWeapon.weapon.reloadSound);
			}
		}

		if (currentState == State.RELOADING)
		{
			int frameNr = (int) (reloadTimer / animationSpeed);

			if (frameNr < (currentWeapon.weapon.reloadSprites.Count))
			{
				Sprite targetSprite = currentWeapon.weapon.reloadSprites[frameNr];

				if (weaponSprite.sprite != targetSprite)
					weaponSprite.sprite = targetSprite;

				reloadTimer += Time.deltaTime;
			}
			else
				Idle();
		}
	}

	public void Shoot()
	{
		if ((currentWeapon.weapon.clipSize == 0 || currentWeapon.currentAmmo > 0) && currentState == State.IDLE)
		{
			if (currentWeapon.weapon.clipSize > 0)
			{
				currentWeapon.currentAmmo--;
				SetAmmoCounterText();
				PlayerStats.Me().bullets++;
			}

			audioSource?.PlayOneShot (currentWeapon.weapon.shootSound);
			currentState = State.SHOOTING;
			shootTimer = 0f;

			Transform cameraTrans = Camera.main.transform;
			Ray ray = new Ray (cameraTrans.position, cameraTrans.forward);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, currentWeapon.weapon.range))
			{
				float dmgMod = currentWeapon.weapon.rangeMod.Evaluate (hit.distance / currentWeapon.weapon.range);
				Damageable tmp = hit.transform.gameObject.GetComponent<Damageable>();
				tmp?.ApplyDamage (currentWeapon.weapon.dmg * dmgMod);
			}
		}

		if (currentState == State.SHOOTING)
		{
			int frameNr = (int) (shootTimer / animationSpeed);

			if (frameNr < (currentWeapon.weapon.shootSprites.Count))
			{
				Sprite targetSprite = currentWeapon.weapon.shootSprites[frameNr];

				if (weaponSprite.sprite != targetSprite)
					weaponSprite.sprite = targetSprite;

				shootTimer += Time.deltaTime;
			}
			else
				Idle();
		}
	}

	//Private

    // Start is called before the first frame update
    void Start()
    {
		InitWeapons();
		SwitchWeapon (0);
		animationSpeed = 1f / sampleRate;
		audioSource = GetComponent <AudioSource>();
		inventory = GetComponent <Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
		{
			case State.IDLE:
				Idle();
				break;
			case State.SHOOTING:
				Shoot();
				break;
			case State.RELOADING:
				Reload();
				break;
		}
    }

	private void InitWeapons()
	{
		weaponContainers = new List<WeaponContainer>();
		foreach (Weapon i in weapons)
			weaponContainers.Add (new WeaponContainer (i));
	}

	private void SwitchWeapon()
	{
		if (switchWeaponTimer >= switchWeaonDelay)
		{

			float mouseDelta = Input.mouseScrollDelta.y;

			if (mouseDelta != 0f)
			{
				int newIndex = currentWeaponIndex;

				if (mouseDelta < 0f)
					newIndex++;
				else
					--newIndex;

				if (newIndex < 0)
					newIndex = weaponContainers.Count -1;
				else if (newIndex >= weaponContainers.Count)
					newIndex = 0;

				SwitchWeapon (newIndex);
			}
			else
			{
				for (int i = 0; i < weaponContainers.Count; ++i)
				{
					int keycode = (int)(KeyCode.Alpha1) + i;

					if (Input.GetKeyDown ((KeyCode) keycode))
					{
						SwitchWeapon (i);
						break;
					}
				}

			}
		}
		else
			switchWeaponTimer += Time.deltaTime;
		
	}

	private void SwitchWeapon (int newWeapon)
	{
		if (newWeapon < weaponContainers.Count)
		{
			switchWeaponTimer = 0f;
			currentWeaponIndex = newWeapon;
			currentWeapon = weaponContainers[newWeapon];
			weaponSprite.sprite = currentWeapon.weapon.idlSprite;
			weaponSprite.SetNativeSize();
			weaponSprite.transform.localPosition = Vector3.right * currentWeapon.weapon.xOffset;

			SetAmmoCounterText();
		}
	}

	void SetAmmoCounterText()
	{
		if (currentWeapon.weapon.clipSize > 0)
			txtAmmoCounter.text = currentWeapon.currentAmmo.ToString();
		else
			txtAmmoCounter.text = "∞";
	}
}
