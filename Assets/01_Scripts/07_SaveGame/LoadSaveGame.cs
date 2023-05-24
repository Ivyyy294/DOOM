using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ivyyy.SaveGameSystem;

public class LoadSaveGame : MonoBehaviour
{
	bool loaded = false;

	// Update is called once per frame
	private void Start()
	{
		loaded = false;
	}

	void Update()
    {
		if (!loaded)
		{
			bool loadGame = PlayerStats.Me().loadGameOnStart;

			PlayerStats.Me().Init();
			PlayerStats.Me().Reset();
			
			if (loadGame)
				SaveGameManager.Me().LoadGameState();
			
			loaded = true;
		}
    }
}
