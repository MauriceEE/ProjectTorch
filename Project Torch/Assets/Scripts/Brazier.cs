using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class exists in case braziers need it... but at present it doesn't do much
/// TODO: Lit/unlit animations
/// </summary>
public class Brazier : MonoBehaviour {

    #region Public Fields
    //Zone this brazier is in
    public ZoneManager.ZoneNames zone;
    //Whether or not the brazier is lit
    public bool lit;
    #endregion

    #region Custom Methods
    /// <summary>
    /// Sets brazier as lit or unlight
    /// Also updates in the flag manager
    /// TODO: animations or something
    /// 
    /// Returns dialogue name, or null if 
    /// </summary>
    public string IgniteBrazier()
    {
        lit = !lit;
        Debug.Log("Brazier: " + lit);
        GameObject.Find("FlagManagerGO").GetComponent<FlagManager>().BrazierLit(zone, lit);

        if(lit) GetComponent<SpriteRenderer>().color = new Color((255f/255f), (184f/255f), (184f/255f));
        else GetComponent<SpriteRenderer>().color = Color.white;

        switch (zone)
        {
            case ZoneManager.ZoneNames.Battlefield:
                return "Brazier - Battlefield";
            case ZoneManager.ZoneNames.HumanTerritoryStage1:
                return "Brazier - HumanTerritoryStage1";
            case ZoneManager.ZoneNames.HumanTerritoryStage2:
                return "Brazier - HumanTerritoryStage2";
            case ZoneManager.ZoneNames.ShadowTerritoryStage1:
                return "Brazier - ShadowTerritoryStage1";
            case ZoneManager.ZoneNames.WarZone:
                return "Brazier - WarZone";
            case ZoneManager.ZoneNames.WarZoneStage2:
                return "Brazier - WarZoneStage2";
            case ZoneManager.ZoneNames.TrueHumanStage1:
                return "Brazier - TrueHumanStage1";

            default:
                return null;
        }
    }
    #endregion
}