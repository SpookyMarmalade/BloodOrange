using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawnControl : NetworkManager {

    public GameObject monsterPrefab;

	private bool hasMonster = false;

	// Use this for initialization
	void Start () {
		
	}
	
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerIdentity){
		Vector3 spawnPoint = GetStartPosition ().position;
		GameObject player;

		if (!hasMonster) {
			player = (GameObject)Instantiate (monsterPrefab, spawnPoint, Quaternion.identity);
			hasMonster = true;
		} else {
			player = (GameObject)Instantiate<GameObject>(playerPrefab, spawnPoint, Quaternion.identity);
		}

		NetworkServer.AddPlayerForConnection (conn, player, playerControllerIdentity);
	}
}
