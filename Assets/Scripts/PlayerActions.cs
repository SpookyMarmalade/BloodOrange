using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerActions : NetworkBehaviour {
    
	[SyncVar(hook = "UpdateSprite")]
	public int playernumber;

	[SerializeField]
	Sprite[] playerAvatars;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateSprite(int spriteID){
		if (playerAvatars.Length > 0) {
			int avatarID = spriteID % playerAvatars.Length;
			GetComponent<SpriteRenderer> ().sprite = playerAvatars [avatarID];
		}
	}
}
