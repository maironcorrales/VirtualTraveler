using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_RecticleLookAt : MonoBehaviour {

    public GameObject Camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.LookAt(Camera.transform);
	}
}
