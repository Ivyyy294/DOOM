using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	Vector3 directionToPlayer;
    
	// Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        directionToPlayer = Camera.main.transform.position - transform.position;
		directionToPlayer.y = 0f;
		transform.forward = directionToPlayer;
    }
}
