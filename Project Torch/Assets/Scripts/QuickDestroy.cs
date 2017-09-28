using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Literally all this does is destroy something soon after it's made
/// For testing purposes only at the moment
/// </summary>
public class QuickDestroy : MonoBehaviour {
	void Start () { Destroy(this.gameObject, 0.02f); }
}
