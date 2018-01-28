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
}
