using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			PlayerStats.Me().Init();
			
			if (PlayerStats.Me().loadGameOnStart)
			{
				PlayerStats.Me().Reset();
				SaveGameManager.Me().LoadGameState();
			}
			
			loaded = true;
		}
    }
}
