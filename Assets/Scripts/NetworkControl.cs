using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkControl : NetworkManager{

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerIdentity){
		Vector3 spawnPoint = GetStartPosition ().position;
		GameObject player;

		player = (GameObject)Instantiate<GameObject> (playerPrefab, spawnPoint, Quaternion.identity);

		GameControl.singleton.RegisterPlayer (player);

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerIdentity);
	}

}
