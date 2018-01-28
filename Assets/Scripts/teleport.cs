using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour {

    [SerializeField]
    GameObject hole;
    [SerializeField]
    float dist;
    bool fin = false;
    Vector3 target;
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
        if (!playerActions.metadata.Contains("tablet!"))
            return;
        playerActions.metadata.Replace("tablet!", "");
        if (fin)
            return;
        hole.SetActive(false);
        target = this.transform.position + Vector3.right * dist;
        fin = true;
    }
    // Update is called once per frame
    void Update () {
		
        if (fin && target != this.transform.position)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, target, 0.01f);
        }
	}
}
