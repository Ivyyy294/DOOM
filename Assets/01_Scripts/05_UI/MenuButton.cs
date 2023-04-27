using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
	[SerializeField] GameObject imgSkull;
	//[SerializeField] UnityEvent onMouseClick;

	private void Start()
	{
		imgSkull?.SetActive (false);
	}

	public void SetImage (bool val)
	{
		imgSkull.SetActive (val);
	}
}
