using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemies : MonoBehaviour {
    public BackgroundManager bg;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		foreach(Transform child in gameObject.transform) {
                bg.MoveByBackgroundOffset(child);
        }
	}

}
