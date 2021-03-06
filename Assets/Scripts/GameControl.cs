﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameControl : NetworkBehaviour {

	public GameObject monsterPrefab;
	public GameObject luckyPrefab;
	public GameObject paigePrefab;
	public GameObject teddyPrefab;
	public GameObject pennyPrefab;

    public AudioMixer mixer;
    public AudioSource infectionAudioSource;
    private AudioMixerSnapshot normalSnapshot;
    private AudioMixerSnapshot infectionSnapshot;
    private bool infectionPlaying = false;

	public static GameControl singleton;

	int playerCount = 0;
	bool started = false;

	// Use this for initialization
	void Start() {
		if (!isServer) return;
		singleton = this;
        normalSnapshot = mixer.FindSnapshot("Snapshot");
        infectionSnapshot = mixer.FindSnapshot("InfectionSnapshot");
	}

    private void Update()
    {
        if (infectionAudioSource.isPlaying) {
            infectionPlaying = true;
        }
        if (infectionPlaying && !infectionAudioSource.isPlaying) {
            normalSnapshot.TransitionTo(1f);
            infectionPlaying = false;
        }
    }
    void FixedUpdate()
	{
		if (!isServer) return;
		if (!started && playerCount >= 2) {			
			started = true;
		}

		if (started && playerCount <= 1)
		{
			Debug.Log("Game Over!");
		}
	}

	public void InfectPlayer(PlayerActions other){

		Debug.Log ("infecting player");
		GameObject player = Instantiate<GameObject> (monsterPrefab, other.transform.position, Quaternion.identity);

		var conn = other.connectionToClient;

		player.GetComponent<MonsterActions> ().contextPrompt = other.contextPrompt;

        infectionSnapshot.TransitionTo(0.5f);
        infectionAudioSource.Play();

		Destroy (other.gameObject);

		bool rep = NetworkServer.ReplacePlayerForConnection (conn, player, 0);

		Debug.Log (rep);
	}

	public void ReplacePlayer(PlayerActions other, int pid){
		Debug.Log ("Reforming player");

		GameObject prefab = (pid == 1 ? luckyPrefab : (pid == 3 ? paigePrefab : (pid == 4 ? teddyPrefab : pennyPrefab)));

		GameObject player = Instantiate<GameObject> (prefab, other.transform.position, Quaternion.identity);

		var conn = other.connectionToClient;

		player.GetComponent<PlayerActions> ().contextPrompt = other.contextPrompt;

		Destroy (other.gameObject);

		bool rep = NetworkServer.ReplacePlayerForConnection (conn, player, 0);

		Debug.Log (rep);
	}

	public void RegisterPlayer(GameObject player){		
		playerCount++;
		if (playerCount == 2) {
			InfectPlayer (player.GetComponent<PlayerActions> ());
		} else {
			ReplacePlayer (player.GetComponent<PlayerActions> (), playerCount);
		}

	}
}
