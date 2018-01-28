using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MonsterActions :NetworkBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Grrrr...");
		if (null != other.GetComponentInParent<DoorBehaviour> ()) {
			Debug.Log ("Out of the way!");
			DoorBehaviour door = other.GetComponentInParent<DoorBehaviour> ();
			door.ToggleDirection ();
		} else if (null != other.GetComponentInParent<PlayerActions> ()) {
			if (!isServer)
				return;
			Debug.Log ("Mwahahaha!");
			GameControl.singleton.InfectPlayer (other.GetComponentInParent<PlayerActions> ());
		}
	}

}
