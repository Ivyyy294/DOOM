using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class WeaponContainer
{
	public Weapon weapon;
	public int currentAmmo;
	public bool active;
}

public class WeaponManager : MonoBehaviour
{
	public List <WeaponContainer> weaponContainers;
	[SerializeField] int sampleRate = 1;
	[SerializeField] float switchWeaonDelay;

	[Header ("Lara Values")]
	[SerializeField] Image weaponSprite;
	[SerializeField] TextMeshProUGUI txtAmmoCounter;
	[SerializeField] TextMeshProUGUI[] txtWIndex;
	[SerializeField] Color32 wActive;
	[SerializeField] Color32 wInactive;
	[SerializeField] RectTransform switchPos;

	enum State
	{
		IDLE,
		SHOOTING,
		RELOADING,
		SWITCH_WEAPON_DOWN,
		SWITCH_WEAPON_UP,
	}

	public int currentWeaponIndex;

	private float animationSpeed;
	private State currentState;
	private WeaponContainer currentWeapon;
	private float reloadTimer;
	private float shootTimer;
	private AudioSource audioSource;
	private Inventory inventory;
	private int newWeaponIndex;

	//Public
	public void UnlockWeapon (Weapon w)
	{
		for (int i = 0; i < weaponContainers.Count; ++i)
		{
			if (weaponContainers[i].weapon == w)
				weaponContainers[i].active = true;
		}
	}

	public void SetCurrentWeaponIndex (int val)
	{
		SwitchWeapon (val);
		SetAmmoCounterText();
		weaponSprite.transform.localPosition = Vector3.right * currentWeapon.weapon.xOffset;
	}

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
			CheckSwitchWeaponInput();

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
				int available = inventory.GetAmmoForReloading (currentWeapon.weapon.ammoTyp, currentWeapon.weapon.clipSize - currentWeapon.currentAmmo);
				currentWeapon.currentAmmo += available;
			}
			else
				currentWeapon.currentAmmo = currentWeapon.weapon.clipSize;
			
			SetAmmoCounterText();

			if (currentWeapon.currentAmmo > 0)
			{
				currentState = State.RELOADING;
				reloadTimer = 0f;
				PlaySoundEffect (currentWeapon.weapon.reloadSound);
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
		bool hasAmmo = currentWeapon.weapon.clipSize > 0;

		if ((!hasAmmo || currentWeapon.currentAmmo > 0) && currentState == State.IDLE)
		{
			if (hasAmmo)
			{
				PlayerStats.Me().shotsFired++;
				currentWeapon.currentAmmo--;
				SetAmmoCounterText();
			}
			

			PlaySoundEffect (currentWeapon.weapon.shootSound);
			currentState = State.SHOOTING;
			shootTimer = 0f;

			Transform cameraTrans = Camera.main.transform;
			Ray ray = new Ray (cameraTrans.position, cameraTrans.forward);

			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, currentWeapon.weapon.range))
			{
				float dmgMod = currentWeapon.weapon.rangeMod.Evaluate (hit.distance / currentWeapon.weapon.range);
				Damageable tmp = hit.transform.gameObject.GetComponent<Damageable>();

				if (tmp != null)
				{
					tmp.ApplyDamage (currentWeapon.weapon.dmg * dmgMod);

					if (hasAmmo)
						PlayerStats.Me().hits++;
				}
			}
		}
		else if (hasAmmo && currentState == State.IDLE && currentWeapon.currentAmmo <= 0)
			PlaySoundEffect (currentWeapon.weapon.emptySound);

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

		animationSpeed = 1f / sampleRate;
		audioSource = GetComponent <AudioSource>();
		inventory = GetComponent <Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale > 0f)
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
				case State.SWITCH_WEAPON_DOWN:
					MoveWeaponDown();
					break;
				case State.SWITCH_WEAPON_UP:
					MoveWeaponUp();
					break;
			}
		}
    }

	private void InitWeapons()
	{
		currentWeaponIndex = -1;
		SetCurrentWeaponIndex (0);
	}

	private void CheckSwitchWeaponInput()
	{
		if (currentState == State.IDLE)
		{

			float mouseDelta = Input.mouseScrollDelta.y;

			if (mouseDelta != 0f)
			{
				int newIndex = currentWeaponIndex;

				if (mouseDelta < 0f)
					newIndex = GetNextValidWeapon (newIndex, 1);
				else
					newIndex = GetNextValidWeapon (newIndex, -1);

				if (currentWeaponIndex != newIndex)
				{
					newWeaponIndex = newIndex;
					currentState = State.SWITCH_WEAPON_DOWN;
				}
			}
			else
			{
				for (int i = 0; i < weaponContainers.Count; ++i)
				{
					int keycode = (int)(KeyCode.Alpha1) + i;

					if (Input.GetKeyDown ((KeyCode) keycode))
					{
						if (currentWeaponIndex != i && WeaponAvailable(i))
						{
							newWeaponIndex = i;
							currentState = State.SWITCH_WEAPON_DOWN;
						}
						break;
					}
				}

			}
		}
	}

	private void MoveWeaponDown()
	{
		Vector2 targetPos = switchPos.anchoredPosition;
		MoveWeapon (Vector2.right * currentWeapon.weapon.xOffset, targetPos);

		float distance = Vector2.Distance (weaponSprite.rectTransform.anchoredPosition, targetPos);

		if (weaponSprite.rectTransform.anchoredPosition == targetPos)
		{
			currentState = State.SWITCH_WEAPON_UP;
			SwitchWeapon (newWeaponIndex);
		}
	}

	private void MoveWeaponUp()
	{
		Vector2 targetPos = Vector2.right * currentWeapon.weapon.xOffset;
		MoveWeapon (switchPos.anchoredPosition, targetPos);

		if (weaponSprite.rectTransform.anchoredPosition == targetPos)
			currentState = State.IDLE;
	}

	private void MoveWeapon (Vector2 startPos, Vector2 targetPos)
	{
		//Base values
		float animationTime = switchWeaonDelay * 0.5f;
		float distance = Vector2.Distance (startPos, targetPos);
		float speed = distance * (1f / animationTime) * Time.deltaTime;

		distance = Vector2.Distance (weaponSprite.rectTransform.anchoredPosition, targetPos);

		if (distance > speed)
		{
			Vector2 direction = (targetPos - weaponSprite.rectTransform.anchoredPosition).normalized;
			weaponSprite.rectTransform.anchoredPosition += direction * speed;
		}
		else
			weaponSprite.rectTransform.anchoredPosition = targetPos;
	}

	private int GetNextValidWeapon (int startIndex, int sign)
	{
		sign = (int)Mathf.Sign (sign);
		int newIndex = startIndex;

		do
		{
			newIndex += sign;

			if (newIndex < 0)
				newIndex = weaponContainers.Count -1;
			else if (newIndex >= weaponContainers.Count)
				newIndex = 0;
		}
		while (!WeaponAvailable (newIndex));

		return newIndex;
	}

	private bool WeaponAvailable (int weapon)
	{
		return weapon < weaponContainers.Count && weaponContainers[weapon].active;
	}

	private void SwitchWeapon (int newWeapon)
	{
		if (currentWeaponIndex != -1)
			SetTxtWIndexColor (currentWeaponIndex, false);

		if (newWeapon < weaponContainers.Count)
		{
			currentWeaponIndex = newWeapon;
			currentWeapon = weaponContainers[newWeapon];
			weaponSprite.sprite = currentWeapon.weapon.idlSprite;
			weaponSprite.SetNativeSize();
			SetAmmoCounterText();
			SetTxtWIndexColor (newWeapon, true);
		}
	}

	void SetAmmoCounterText()
	{
		if (currentWeapon.weapon.clipSize > 0)
			txtAmmoCounter.text = currentWeapon.currentAmmo.ToString();
		else
			txtAmmoCounter.text = "∞";
	}

	void SetTxtWIndexColor (int index, bool active)
	{
		if (index < txtWIndex.Length)
			txtWIndex[index].color = active ? wActive : wInactive;
	}

	void PlaySoundEffect(AudioClip clip)
	{
		if (clip != null && audioSource != null)
			audioSource.PlayOneShot (clip);
	}
}
