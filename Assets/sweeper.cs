using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweeper : MonoBehaviour {
    [SerializeField]
    Vector3 target1, target2;
    Vector3 currenttarget;
    bool istarget1 = true;
    float duration;
    // Use this for initialization
    void Start () {
        currenttarget = target1;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, currenttarget, duration);
        if (this.transform.position == currenttarget)
        {
            currenttarget = istarget1 ? target2 : target1;
            istarget1 = !istarget1;
        }
	}
}
