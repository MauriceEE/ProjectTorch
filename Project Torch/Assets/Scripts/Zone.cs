using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class exists solely to hold data for the zone manager to use
/// </summary>
public class Zone : MonoBehaviour {
    //The name of this zone (for easy lookup in zone manager)
    public ZoneManager.ZoneNames zone;
    //The endpoint object of this zone (to manage screen transitions)
    public GameObject endPoint;
}
