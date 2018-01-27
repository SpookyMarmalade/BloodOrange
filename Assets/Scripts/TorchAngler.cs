using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TorchAngler : MonoBehaviour {
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        PlayerMovement playerMovement = ClientScene.localPlayers[0].gameObject.GetComponent<PlayerMovement>();


        Debug.Log(Vector2.Angle(Vector2.down, playerMovement.velocity).ToString());
        transform.rotation = Quaternion.Euler(0, 0, 0 - Vector2.Angle(Vector2.up, playerMovement.velocity));

    }
}
