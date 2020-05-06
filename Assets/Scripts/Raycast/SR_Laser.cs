using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SR_Laser : MonoBehaviour {

    public Transform startPoint;
    public Transform endPoint;

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<LineRenderer>().startWidth = 0.01f;
        this.gameObject.GetComponent<LineRenderer>().endWidth = 0.01f;
    }
	
	// Update is called once per frame
	void Update () {
        this.gameObject.GetComponent<LineRenderer>().SetPosition(0, startPoint.position);
        this.gameObject.GetComponent<LineRenderer>().SetPosition(1, endPoint.position);
    }
}
