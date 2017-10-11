using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZLock : MonoBehaviour {
    public float positionZ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, positionZ);
	}
}
