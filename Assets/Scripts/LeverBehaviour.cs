using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverBehaviour : MonoBehaviour {

	public interface LeverTrigger{

		void TriggerEffect (GameObject activatingPlayer);

		void UnTriggerEffect (GameObject activatingPlayer);
	}

	public LeverTrigger triggerTarget;
	public bool startIsOn;
	private bool isOn;

	// Use this for initialization
	void Start () {
		isOn = startIsOn;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void Animate(){

	}

	public void TogglePosition(GameObject activatingPlayer){
		isOn = !isOn;
		if (isOn) {
			triggerTarget.TriggerEffect (activatingPlayer);
		} else {
			triggerTarget.UnTriggerEffect (activatingPlayer);
		}
	}
}
