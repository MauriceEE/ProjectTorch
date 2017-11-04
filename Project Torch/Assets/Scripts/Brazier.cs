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
    /// </summary>
    /// <param name="_lit">Whether or not the brazier should be lit</param>
    public void IgniteBrazier(bool _lit)
    {
        //lit = _lit;
        lit = !lit;
        Debug.Log("Brazier: " + lit);
        GameObject.Find("FlagManagerGO").GetComponent<FlagManager>().BrazierLit(zone, lit);

        if(lit) GetComponent<SpriteRenderer>().color = new Color((255f/255f), (184f/255f), (184f/255f));
        else GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion
}