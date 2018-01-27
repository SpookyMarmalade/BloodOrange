using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterActions :NetworkBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		if (!isServer) return;
		Debug.Log ("BOOP" + other.GetComponentInParent<PlayerActions>());
		PlayerSpawnControl.singleton.InfectPlayer(other.GetComponentInParent<PlayerActions>());
	}

}
