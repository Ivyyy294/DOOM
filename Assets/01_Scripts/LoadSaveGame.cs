using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveGame : MonoBehaviour
{
	bool loaded = false;

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.Me().loadGameOnStart && !loaded)
		{
			SaveGameManager.Me().LoadGameState();
			loaded = true;
		}
    }
}
