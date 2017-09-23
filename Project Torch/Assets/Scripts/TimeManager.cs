using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public Text timeText;

    //Public so that you can watch it in the inspector
    public float torchTime;

	void Start () {
        torchTime = 3600.000f; //1 hour in seconds
        timeText.text = "Torch Time: \n" + torchTime.ToString();
    }
	
	void Update () {
        torchTime -= Time.deltaTime;
        timeText.text = "Torch Time: \n" + torchTime.ToString();
    }
}