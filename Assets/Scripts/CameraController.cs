using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
	
	void Update () {

        Transform playerTransform = ClientScene.localPlayers[0].gameObject.transform;
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
	}
}
