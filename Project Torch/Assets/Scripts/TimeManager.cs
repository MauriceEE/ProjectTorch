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
    // torch run out display phrase
    public string torchOutText;
    #endregion

    #region Private Fields
    private int torchTimeMinutes;
    private float torchTimeSeconds;
    #endregion

    #region Unity Methods
    void Start () {
        //torchTime = 900f; //15 mins in seconds
        torchTimeMinutes = Mathf.FloorToInt(torchTime / 60);
        torchTimeSeconds = Mathf.RoundToInt(torchTime - torchTimeMinutes * 60);
        timeText.text = "Torch Time: \n" + torchTimeMinutes.ToString() + "  mins  " + torchTimeSeconds.ToString() + "  secs";
    }
	void Update () {

        // update color
        timeText.CrossFadeColor(Color.red, torchTime, false, false);

        if (torchTime > 0)
        {
            // subtract delta time and parse min and sec values
            torchTime -= Time.deltaTime;
            torchTimeMinutes = Mathf.FloorToInt(torchTime / 60);
            torchTimeSeconds = Mathf.Clamp(Mathf.RoundToInt(torchTime - torchTimeMinutes * 60), 0, 59);

            // display the time
            timeText.text = "Torch Time: \n" + torchTimeMinutes.ToString() + "  mins  " + torchTimeSeconds.ToString() + "  secs";
        }
        else // time is zero
        {
            // set time to zero
            torchTimeMinutes = 0;
            torchTimeSeconds = 0;

            // display flame expired text
            timeText.text = torchOutText;
        }
    }
#endregion
}