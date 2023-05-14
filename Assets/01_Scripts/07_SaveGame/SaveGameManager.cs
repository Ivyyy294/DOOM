using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class Payload
{
	public string id;
	public Dictionary <string, string> data = new Dictionary <string, string>();

	public Payload (string uniqueId) {id = uniqueId;}

	public void Add (string key, string val) {data.Add (key, val);}
	public void Add (string key, float val) {data.Add (key, val.ToString());}
	public void Add (string key, int val) {data.Add (key, val.ToString());}
	public void Add (string key, bool val) {data.Add (key, val.ToString());}

	public string GetSerializedData()
	{
		return JsonConvert.SerializeObject (this);
	}

	public static Payload GetData (string line)
	{
		Payload val = JsonConvert.DeserializeObject <Payload> (line);
		return val;
	}
}

public class SaveGameManager
{
	//Instance
	private static SaveGameManager me;
	
	public static SaveGameManager Me()
	{
		if (me == null)
			me = new SaveGameManager();

		return me;
	}

	private string filePath;
	private Dictionary <string, Payload> ObjectDataList;

	public SaveGameManager()
	{
		filePath = Application.persistentDataPath + "/savefile.txt";
		Debug.Log ("Will save to " + filePath);
	}

	public void SaveGameState()
	{
		StreamWriter writer = new StreamWriter (filePath);

		foreach (var i in SaveableObject.allSaveableObject)
			writer.WriteLine (i.Value.GetSerializedData());

		writer.Close();
		Debug.Log ("Save to " + filePath);
	}

	public void LoadGameState()
	{
		LoadObjectDataList();
		
		foreach (var i in SaveableObject.allSaveableObject)
		{
			if (ObjectDataList.ContainsKey (i.Key))
				i.Value.LoadObject(ObjectDataList[i.Key]);
		}

		Debug.Log ("Load");
	}

	void LoadObjectDataList()
	{
		ObjectDataList = new Dictionary<string, Payload>();
		StreamReader reader = new StreamReader (filePath);

		for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
		{
			Payload payload = Payload.GetData (line);
			ObjectDataList[payload.id] = payload;
		}

		reader.Close();
	}
}