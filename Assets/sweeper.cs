using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sweeper : MonoBehaviour {
    [SerializeField]
    Vector3 target1, target2;
    [SerializeField]
    float duration;
    bool movetotarget1 = true;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, (movetotarget1? target1:target2), duration);
        if (Vector3.Distance(this.transform.position, target1) < 0.01f)
        {
            movetotarget1 = false;
        }
        if (Vector3.Distance(this.transform.position, target2) < 0.01f)
        {
            movetotarget1 = true;
        }

    }
}
