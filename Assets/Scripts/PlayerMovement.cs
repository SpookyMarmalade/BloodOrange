﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using InControl;

public class PlayerMovement : NetworkBehaviour {

    public float moveSpeed;
    private const float sqrtHalf = 0.7071067811865476f;
    public Vector2 velocity;
    private Rigidbody2D rb;
    public GameController gc;

    // Use this for initialization
    void Start () {
        rb = transform.GetComponent<Rigidbody2D>();
	}

    public override void OnStartLocalPlayer(){
        GetComponent<SpriteRenderer>().material.color = Color.green;
        gc.LocalPlayer = gameObject;
    }

	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer) return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (InputManager.Devices.Count > 0) {
            //override with dpad if being used
            if (Mathf.Abs(InputManager.ActiveDevice.DPad.X) > 0) x = InputManager.ActiveDevice.DPad.X;
            if (Mathf.Abs(InputManager.ActiveDevice.DPad.Y) > 0) y = InputManager.ActiveDevice.DPad.Y;

            //or thumbstick if that's being used
            if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX) > 0) x = InputManager.ActiveDevice.LeftStickX;
            if (Mathf.Abs(InputManager.ActiveDevice.LeftStickY) > 0) y = InputManager.ActiveDevice.LeftStickY;
        }

        //normalised to get around diagonal being faster
        velocity = new Vector2(x, y).normalized * moveSpeed;
        rb.velocity = velocity;

        //transform.Translate(x,y,z);
	} 
}
