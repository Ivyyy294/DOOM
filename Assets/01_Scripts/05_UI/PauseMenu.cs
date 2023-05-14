using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] GameObject crossHair;
	[SerializeField] GameObject weaponImage;

	public void Continue()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f;
		crossHair.SetActive (true);
		weaponImage.SetActive (true);
		gameObject.SetActive (false);
	}

	public void Abort()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene (2);
	}

	public void Quit()
	{
		Application.Quit();
	}

	private void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Time.timeScale = 0f;
		crossHair.SetActive (false);
		weaponImage.SetActive (false);
	}
}
