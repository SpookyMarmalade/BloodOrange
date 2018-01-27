using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TorchAngler : MonoBehaviour {
    
	void Update () {

        if (ClientScene.localPlayers.Count > 0) {
            //set torch to point in the right direction based on player movement
            PlayerMovement playerMovement = ClientScene.localPlayers[0].gameObject.GetComponent<PlayerMovement>();
            if (Mathf.Abs(playerMovement.velocity.magnitude) > 0) {
                if (playerMovement.velocity.x > 0) {
                    transform.rotation = Quaternion.Euler(0, 0, 180 - Vector2.Angle(Vector2.up, playerMovement.velocity));
                } else {
                    transform.rotation = Quaternion.Euler(0, 0, 180 + Vector2.Angle(Vector2.up, playerMovement.velocity));
                }
            }
        }
    }
}
