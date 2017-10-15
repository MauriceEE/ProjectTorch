using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    #endregion

    #region Unity Defaults
    void Start () {
        currentZone = Zones.Battlefield;
	}
	
	void Update () {
		
	}
#endregion
}