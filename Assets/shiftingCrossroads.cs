using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shiftingCrossroads : MonoBehaviour {
    [SerializeField]
    GameObject setting1, setting2;
    [SerializeField]
    int interval;
    [SerializeField]
    float duration;
    Vector3 target1, target2;
    int prev;
    [SerializeField]
    float dist;
	// Use this for initialization
	void Start ()
    {
        target1 = setting1.transform.position;
        target2 = setting2.transform.position + Vector3.right * dist;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        setting1.transform.position = Vector3.Lerp(setting1.transform.position, target1, duration);
        setting2.transform.position = Vector3.Lerp(setting2.transform.position, target2, duration);
        int current = (int)Time.time / interval;
        if (current > prev)
        {
            prev = current;
            if (current % 2 == 0)
            {
                target1 = setting1.transform.position + Vector3.down * dist;
                target2 = setting2.transform.position + Vector3.right * dist;
            }
            else
            {
                target1 = setting1.transform.position + Vector3.up * dist;
                target2 = setting2.transform.position + Vector3.left * dist;
            }
        }

	}
}
