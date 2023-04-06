using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerKey
{
	private bool isActive = false;

	public void SetActive (bool val) { isActive = val; }
	public bool IsActive () { return isActive;}
}

public class PlayerKeyChain : MonoBehaviour
{
	//private bool KeyRoom1 {public get; public set};

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
