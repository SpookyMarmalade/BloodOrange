using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

	public int speed = 5;

	private bool moving = false;
	private bool opening = false;

	private Vector3 closed;
	private float openAngle = 90f;
	public Transform hinge;

	private float target;

	// Use this for initialization
	void Start () {
		closed = hinge.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			MoveOnHinge ();
			if ((opening ? 1 : -1) * target <=0) {
				moving = false;
			}
		}
	}

	void MoveOnHinge(){
		float change =(opening ? 1 : -1)* Time.deltaTime * speed;
		target -= change;
		transform.RotateAround (hinge.position, new Vector3(0,0,1), change);
	}

	public void ToggleDirection(){
		if (moving)
			return;
		opening = !opening;
		moving = true;
		target = opening ? openAngle : -openAngle;
	}
}
