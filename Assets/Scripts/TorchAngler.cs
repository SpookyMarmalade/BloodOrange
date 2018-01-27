using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TorchAngler : MonoBehaviour {
    private Vector2 torchDir = Vector2.right;

	void Update () {

        if (ClientScene.localPlayers.Count > 0) {
            //set torch to point in the right direction based on player movement
            PlayerMovement playerMovement = ClientScene.localPlayers[0].gameObject.GetComponent<PlayerMovement>();
            if (playerMovement.velocity.magnitude != 0) {
                torchDir = Vector2.Lerp(torchDir, playerMovement.velocity, 0.5f);
                float torchAngle = Vector2.SignedAngle(Vector2.down, torchDir);
                transform.rotation = Quaternion.Euler(0, 0, torchAngle);
            }
        }
    }
}
