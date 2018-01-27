using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterActions :NetworkBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (!isServer) return;
		Debug.Log ("BOOP" + other.GetComponentInParent<PlayerActions>());
		GameControl.singleton.InfectPlayer(other.GetComponentInParent<PlayerActions>());
	}

}
