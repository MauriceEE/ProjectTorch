using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    //Public so that you can watch it in the inspector
    public float torchTime;

	void Start () {
        torchTime = 3600.000f; //1 hour in seconds
    }
	
	void Update () {
        torchTime -= Time.deltaTime;
	}
}