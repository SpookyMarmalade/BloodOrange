using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MonsterActions :NetworkBehaviour {

	public Text contextPrompt;

	[SyncVar]
	public bool isInteracting = false;

	private bool inContext = false;
	private bool isUsing = false;
	private Collider2D contextObject = null;
	private PlayerMovement moving;
	private Animator animator;

	void Start(){
		moving = GetComponent<PlayerMovement> ();
		animator = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (inContext) {
			UpdateContextUI ();
			ContextInteraction ();
			Animate ();
			isUsing = false;
		}
	}

	void UpdateContextUI(){
		contextPrompt.text = "[Space]";
	}
		
	void ContextInteraction(){
		Debug.Log ("Grrrr...");
		//if (!Interacting ())
		//	return;

		isUsing = true;
		if (null != contextObject.GetComponentInParent<DoorBehaviour> ()) {
			Debug.Log ("Out of the way!");
			DoorBehaviour door = contextObject.GetComponentInParent<DoorBehaviour> ();
			door.ToggleDirection ();
		} else if (null != contextObject.GetComponentInParent<PlayerActions> ()) {
			if (!isServer)
				return;
			Debug.Log ("Mwahahaha!");
			GameControl.singleton.InfectPlayer (contextObject.GetComponentInParent<PlayerActions> ());
		}
	}

	private bool Interacting(){	
		if (!isLocalPlayer)
			return isInteracting;
		else 
			return (isInteracting = Input.GetKeyDown (KeyCode.Space));
	}

	void OnTriggerEnter2D(Collider2D other){
		inContext = true;
		contextObject = other;
	}

	void OnTriggerExit2D(Collider2D other){
		inContext = false;
		contextObject = null;
		contextPrompt.text = "";
	}

	void Animate(){
		if (!isLocalPlayer)
			return;
		int state = animator.GetInteger ("state");
		if ((state == 1 || state == 2) && isUsing) {
			animator.SetInteger ("state", 0);
		} else if (state != 3 && isUsing) {
			animator.Play ("use");
		} else if (!isUsing){
			animator.SetInteger ("state", 0);
		}
	}

}
