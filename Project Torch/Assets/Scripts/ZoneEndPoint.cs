using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// At the moment this exists only to hold data about level transitions
/// </summary>
public class ZoneEndPoint : MonoBehaviour {

#region Public Fields
    //Zone to warp the player to
    public ZoneManager.Zones nextZone;
    //New spot for the player to arrive at,
    //only X because we want to keep their Y the same
    public float newXCoordinate;
#endregion
}
