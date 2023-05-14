using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] GameObject crossHair;
	[SerializeField] GameObject weaponImage;
	float timer;
	bool close;

	public void Continue()
	{
		timer = 0f;
		close = true;
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
		close = false;
		Cursor.lockState = CursorLockMode.Confined;
		Time.timeScale = 0f;
		crossHair.SetActive (false);
		weaponImage.SetActive (false);
	}

	private void Update()
	{
		if (close)
		{
			if (timer < 0.1f)
				timer += Time.unscaledDeltaTime;
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Time.timeScale = 1f;
				crossHair.SetActive (true);
				weaponImage.SetActive (true);
				gameObject.SetActive (false);
			}
		}
	}
}
