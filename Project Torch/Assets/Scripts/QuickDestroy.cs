using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Literally all this does is destroy something soon after it's made
/// For testing purposes only at the moment
/// TODO: get rid of this script once we get actual attack animations in
/// </summary>
public class QuickDestroy : MonoBehaviour {
	void Start () { Destroy(this.gameObject, 0.02f); }
}
