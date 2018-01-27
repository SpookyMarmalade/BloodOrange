using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameControl : NetworkBehaviour {

	public GameObject monsterPrefab;

	public static GameControl singleton;

	int playerCount = 0;
	bool started = false;
	private GameObject monsterCandidate;

	// Use this for initialization
	void Start() {
		if (!isServer) return;
		singleton = this;
	}

	void FixedUpdate()
	{
		if (!isServer) return;
		if (!started && playerCount >= 2) {			
			started = true;
			Debug.Log (playerCount);
			PlayerActions mons = monsterCandidate.GetComponent<PlayerActions>();
			Debug.Log (mons.playernumber);
			InfectPlayer(mons);
		}

		if (started && playerCount <= 0)
		{
			Debug.Log("Game Over!");
		}
	}

	public void InfectPlayer(PlayerActions other){

		Debug.Log ("infecting player");
		GameObject player = Instantiate<GameObject> (monsterPrefab, other.transform.position, Quaternion.identity);

		var conn = other.connectionToClient;

		Destroy (other.gameObject);

		bool rep = NetworkServer.ReplacePlayerForConnection (conn, player, 0);

		Debug.Log (rep);

		playerCount--;
	}

	public void RegisterPlayer(GameObject player){		
		player.GetComponent<PlayerActions> ().playernumber = playerCount;
		playerCount++;
		monsterCandidate = player;
	}
}
