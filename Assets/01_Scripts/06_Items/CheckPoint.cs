using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ivyyy.SaveGameSystem;

public class CheckPoint : MonoBehaviour, InteractableObject
{
    public void Interact()
	{
		SaveGameManager.Me().SaveGameState();
		gameObject.SetActive (false);
	}
}
