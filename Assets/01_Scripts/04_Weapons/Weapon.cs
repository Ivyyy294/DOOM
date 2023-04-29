using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Weapon")]
public class Weapon : ScriptableObject
{
	public enum AmmoTyp
	{
		BULLETS,
		SHELLS
	}

	public string displayName;
	public int dmg;
	public int clipSize;
	public AmmoTyp ammoTyp;
	public float range;
	public AnimationCurve rangeMod;

	[Header ("Sprite Settings")]
	public Sprite idlSprite;
	public List <Sprite> shootSprites;
	public List <Sprite> reloadSprites;
	public int xOffset;

	[Header ("Sounds")]
	public AudioClip shootSound;
	public AudioClip reloadSound;
}
