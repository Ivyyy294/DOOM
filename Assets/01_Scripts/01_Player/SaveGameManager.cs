using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
 using System.Linq;

public interface ISaveableObject
{
	public string GetSerializedData ();
	public void LoadObject (string[] data);
}

class Payload
{
	public string id;
	public string data;

	public static Payload GetData (string line)
	{
		Payload val = new Payload();
		string[] tmp = line.Split("</id>");
		val.id = tmp[0];
		val.data = tmp[1];

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
	private Dictionary <string, string> ObjectDataList;

	public SaveGameManager()
	{
		filePath = Application.persistentDataPath + "/savefile.txt";
		Debug.Log ("Will save to " + filePath);
	}

	public void SaveGameState()
	{
		StreamWriter writer = new StreamWriter (filePath);

		var list = Object.FindObjectsOfType<SaveableObject>();

		foreach (SaveableObject i in list)
			writer.WriteLine (i.GetUniqueId() + "</id>" + i.GetSerializedData());

		writer.Close();
		Debug.Log ("Save to " + filePath);
	}

	public void LoadGameState()
	{
		LoadObjectDataList();
		
		var list = Object.FindObjectsOfType<SaveableObject>();

		foreach (SaveableObject i in list)
		{
			if (ObjectDataList.ContainsKey (i.GetUniqueId()))
				i.LoadObject(ObjectDataList[i.GetUniqueId()]);
		}

		Debug.Log ("Load");
	}

	void LoadObjectDataList()
	{
		ObjectDataList = new Dictionary<string, string>();
		StreamReader reader = new StreamReader (filePath);

		for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
		{
			Payload payload = Payload.GetData (line);
			ObjectDataList[payload.id] = payload.data;
		}

		reader.Close();
	}
}
