using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// At the moment this exists only to hold data about level transitions
/// UPDATE: This class is likely completely unnecessary now
/// </summary>
public class ZoneEndPoint : MonoBehaviour {
    //Zone to warp the player to
    public ZoneManager.ZoneNames nextZone;
}
