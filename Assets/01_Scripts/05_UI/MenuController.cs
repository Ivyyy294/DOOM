using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   public void NewGame()
	{
		PlayerStats.Me().loadGameOnStart = false;
		SceneManager.LoadScene (1);
	}

	public void Continue()
	{
		PlayerStats.Me().loadGameOnStart = true;
		SceneManager.LoadScene (1);
	}
}
