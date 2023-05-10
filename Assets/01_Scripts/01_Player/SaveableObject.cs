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
	public static Dictionary <string, SaveableObject> allGuids = new Dictionary <string, SaveableObject>();
	[SerializeField] string uniqueId;

	abstract public string GetSerializedData ();
	abstract public void LoadObject (string data);

	public string GetUniqueId() { return uniqueId; }

	#if UNITY_EDITOR
	void Update()
	{
		// Don't do anything when running the game
		if (Application.isPlaying)
			return;

		// if we are not part of a scene then we are a prefab so do not attempt to set the id
		if  (gameObject.scene.name == null)
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
         allGuids.Remove(uniqueId);
     }
	#endif
}
