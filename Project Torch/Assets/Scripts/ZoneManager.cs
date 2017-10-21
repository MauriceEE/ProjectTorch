using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
/// <summary>
/// Handles zones/zone transition
/// </summary>
public class ZoneManager : MonoBehaviour {
    #region Enums
    public enum Zones
    {
        Battlefield,
        SullenVillage,
        ThrivingVillage,
        CastleOfMan,
        FortressOfDark
    }
    #endregion

    #region Public Fields
    public Zones currentZone;
    #endregion

    #region Private Fields
    private Camera gameCamera;
    private PostProcessChange profileChanger;
    #endregion

    #region Unity Defaults
    void Start () {
        currentZone = Zones.Battlefield;
        gameCamera = Camera.main;
        profileChanger = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessChange>();
	}
	
	void Update () {
		if(currentZone == Zones.ThrivingVillage || currentZone == Zones.FortressOfDark)
        {
            // set darkness to true
            profileChanger.darkness = true;
        }
        else
        {
            // set darkness to false
            profileChanger.darkness = false;
        }
	}
#endregion
}