using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class manages the clock in the background
/// TODO: Game over when time hits zero... flag manager should handle that though
/// </summary>
public class TimeManager : MonoBehaviour {
    #region Public Fields
    //Temporary display for now
    public Text timeText;
    //Public so that you can watch it in the inspector
    public float torchTime;
    #endregion
    #region Unity Methods
    void Start () {
        torchTime = 3600.000f; //1 hour in seconds
        timeText.text = "Torch Time: \n" + torchTime.ToString();
    }
	void Update () {
        torchTime -= Time.deltaTime;
        timeText.text = "Torch Time: \n" + torchTime.ToString();
    }
#endregion
}