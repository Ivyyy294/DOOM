using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Weapon")]
public class Weapon : ScriptableObject
{
	public string displayName;
	public Sprite idlSprite;
	public List <Sprite> shootSprites;
	public List <Sprite> reloadSprites;

	public int clipSize;
	public int dmg;

	[Header ("Sounds")]
	public AudioClip shootSound;
	public AudioClip reloadSound;
}
