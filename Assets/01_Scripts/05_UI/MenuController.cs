using UnityEngine;
using UnityEngine.SceneManagement;
using Ivyyy.SaveGameSystem;

public class MenuController : MonoBehaviour
{
	[SerializeField] GameObject buttonContinue;

	private void Start()
	{
		buttonContinue.SetActive (SaveGameManager.Me().SaveGameAvailable());
	}

	public void NewGame()
	{
		PlayerStats.Me().loadGameOnStart = false;
		SceneManager.LoadScene (2);
	}

	public void Continue()
	{
		PlayerStats.Me().loadGameOnStart = true;
		SceneManager.LoadScene (2);
	}
}
