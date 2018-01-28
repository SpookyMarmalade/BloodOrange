using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addTablet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent == null)
            return;
        PlayerActions playerActions = other.GetComponentInParent<PlayerActions>();
        if (playerActions == null)
            return;
        Debug.Log("Dave");
        playerActions.metadata += "tablet!";
        Destroy(gameObject);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
