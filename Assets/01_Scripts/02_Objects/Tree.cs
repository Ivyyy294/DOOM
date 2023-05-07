using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
	[SerializeField] Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        if (sprites != null)
		{
			int index = Random.Range (0, sprites.Length);
			GetComponent<SpriteRenderer>().sprite = sprites[index];
		}
    }
}
