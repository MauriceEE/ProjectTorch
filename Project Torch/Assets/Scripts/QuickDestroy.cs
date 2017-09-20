using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, 0.02f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
