using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour {

    public float moveSpeed;

    private const float sqrtHalf = 0.7071067811865476f;

	// Use this for initialization
	void Start () {
		
	}

    public override void OnStartLocalPlayer(){
        GetComponent<SpriteRenderer>().material.color = Color.green;
    }

	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer) return;

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        var z = 0.0f;

        //If moving diagonally, normalise speed so magnitude is 1
        if (x != 0 && y != 0){
            x = x * sqrtHalf;
            y = y * sqrtHalf;
        }

        x *= Time.deltaTime * moveSpeed;
        y *= Time.deltaTime * moveSpeed;

        transform.Translate(x,y,z);
	} 
}
