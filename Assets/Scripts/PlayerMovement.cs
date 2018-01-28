using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using InControl;

public class PlayerMovement : NetworkBehaviour {

	public bool moving;
    public float moveSpeed;
    public Vector2 velocity;
    private AudioSource myFootstepsSound;
    private Rigidbody2D rb;
	private Animator animator;

	public float sprintVelocityMultiplier;
	public int maxSprintDuration;
	public int sprintCooldownDuration;
	private int sprintDuration = 0;
	private int cooldownTimer = 0;
	private bool sprinting = false;

    // Use this for initialization
    void Start () {
        rb = transform.GetComponent<Rigidbody2D>();
        myFootstepsSound = Camera.main.GetComponents<AudioSource>()[1];
		animator = GetComponent<Animator> ();
	}

    public override void OnStartLocalPlayer(){
		
    }

	// Update is called once per frame
	void Update () {
		if(!isLocalPlayer) return;

        // Only allow interpolated movement on an axis if the
        // player is currently trying to move along it. This
        // removes the slow deceleration when the player stops.
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0) { x = Input.GetAxis("Horizontal");  }
        if (y != 0) { y = Input.GetAxis("Vertical"); }

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

		//moving?
		moving = (x != 0 || y != 0);

		//sprinting
		if (moving && Input.GetKey (KeyCode.LeftShift) && cooldownTimer <= 0) {
			sprinting = true;
			velocity *= sprintVelocityMultiplier;
			sprintDuration++;
			if (sprintDuration >= maxSprintDuration) {
				cooldownTimer = sprintCooldownDuration;
				sprinting = false;
				sprintDuration = 0;
			}
		} else if (cooldownTimer > 0){
			cooldownTimer--;
		} if (sprinting && Input.GetKeyUp (KeyCode.LeftShift)) {
			cooldownTimer = sprintCooldownDuration;
			sprinting = false;
			sprintDuration = 0;
		}

        rb.velocity = velocity;
		      	
        myFootstepsSound.loop = moving;
        if (moving && !myFootstepsSound.isPlaying) {
            myFootstepsSound.Play();
        }
		if (sprinting && animator.GetInteger ("state") != 2) {
			animator.SetInteger ("state", 2);
		} else if (!sprinting && moving && animator.GetInteger ("state") != 1) {
			animator.SetInteger ("state", 1);
		} else if (!moving && animator.GetInteger ("state") != 0) {
			animator.SetInteger ("state", 0);
		}
	} 
}
