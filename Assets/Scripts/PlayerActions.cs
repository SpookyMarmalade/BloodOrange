using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActions : NetworkBehaviour {
    
	[SyncVar(hook = "UpdateSprite")]
	public int playernumber;

	[SerializeField]
	Sprite[] playerAvatars;
	
	// Update is called once per frame
	void Update () {
		//GetComponent<SpriteRenderer> ().sprite = playerAvatars [0];
	}

	void UpdateSprite(int spriteID){
		Debug.Log ("spr:" + spriteID + " pl:" + playernumber);
		if (playerAvatars.Length > 0) {
			int avatarID = spriteID % playerAvatars.Length;
			GetComponent<SpriteRenderer> ().sprite = playerAvatars [avatarID];
			Debug.Log ("Updated Sprite for " + playernumber);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		Debug.Log ("Ouch!");
		if (null != other.GetComponentInParent<DoorBehaviour>()) {
			Debug.Log ("It's a door!");
			DoorBehaviour door = other.GetComponentInParent<DoorBehaviour> ();
			door.ToggleDirection ();
		} else if (null != other.GetComponentInParent<MonsterActions> ()) {
			if (!isServer)
				return;
			Debug.Log ("AAAAAAAAAAAGGHHHH!");
		}
	}
}
