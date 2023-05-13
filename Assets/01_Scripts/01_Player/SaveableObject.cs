using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
	 using UnityEditor.SceneManagement;
 #endif

[ExecuteInEditMode]
public abstract class SaveableObject : MonoBehaviour
{
	//list of all active ids
	public static Dictionary <string, SaveableObject> allGuids = new Dictionary <string, SaveableObject>();

	//unique stable id
	[HideInInspector]
	[SerializeField] protected string uniqueId;

	//Private
	abstract public Payload GetPayload ();
	abstract public void LoadObject (Payload data);

	public string GetSerializedData()	{return GetPayload().GetSerializedData();}
	public string GetUniqueId() { return uniqueId; }

	//Only runs in Edit mode
	//makes sure every object has a valid uniqueId
	#if UNITY_EDITOR
	void Update()
	{
		// Don't do anything when running the game
		if (Application.isPlaying)
			return;

		// if we are not part of a scene then we are a prefab so do not attempt to set the id
		if  (!IsSceneValid())
			return;

		bool hasValidValue = uniqueId != null && uniqueId.Length > 0;
		bool anotherComponentAlreadyHasThisID = (hasValidValue && allGuids.ContainsKey (uniqueId) && allGuids[uniqueId] != this);

		if (!hasValidValue || anotherComponentAlreadyHasThisID)
		{
			uniqueId = System.Guid.NewGuid().ToString();
			EditorUtility.SetDirty (this);
			EditorSceneManager.MarkSceneDirty (gameObject.scene);
		}

		// We can be sure that the key is unique - now make sure we have 
		// it in our list
		if (!allGuids.ContainsKey (uniqueId))
			allGuids.Add(uniqueId, this);
	}

	void OnDestroy(){
		if (IsSceneValid() && allGuids.ContainsKey (uniqueId))
         allGuids.Remove(uniqueId);
     }
#endif

	//Add Object to Guid list
	private void Start()
	{
		//only add id if we are part of a scene
		if (IsSceneValid() && !allGuids.ContainsKey (uniqueId))
			allGuids.Add(uniqueId, this);
	}

	private bool IsSceneValid()
	{
		return gameObject.scene.name != null && gameObject.scene.buildIndex > -1;
	}
}
