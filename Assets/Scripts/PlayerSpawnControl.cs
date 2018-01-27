using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawnControl : NetworkManager{

	public GameObject monsterPrefab;
	public static PlayerSpawnControl singleton;
	private bool hasMonster = false;
    List<GameObject> players;
    List<GameObject> monsters;
    [SerializeField]
    float infect_time=20;
    int playerNumber = 0;
    bool started = false;
	// Use this for initialization
	void Start() {
		singleton = this;
        players = new List<GameObject>();
        monsters = new List<GameObject>();

    }
    void Update()
    {
        if (!started && infect_time - Time.time <= 0) {
			started = true;
            InfectPlayer(players[(int)UnityEngine.Random.Range(0, players.Count)].GetComponent<PlayerActions>());
        }
        if (started && players.Count <= 0)
        {
            Debug.Log("Game Over!");
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerIdentity){
		Vector3 spawnPoint = GetStartPosition ().position;
		GameObject player;
		
		player = (GameObject)Instantiate<GameObject>(playerPrefab, spawnPoint, Quaternion.identity);
        players.Add(player);
        player.GetComponent<PlayerActions>().playernumber = ++playerNumber;
			
		NetworkServer.AddPlayerForConnection (conn, player, playerControllerIdentity);
	}
		
	public void InfectPlayer(PlayerActions other){
        players.Remove(other.gameObject);
		var it = other.connectionToClient;
		GameObject player = Instantiate<GameObject> (monsterPrefab, other.transform.position, Quaternion.identity);
        monsters.Add(player);
		Destroy (other.gameObject);
		NetworkServer.ReplacePlayerForConnection (it, player, 0);

	}
}
