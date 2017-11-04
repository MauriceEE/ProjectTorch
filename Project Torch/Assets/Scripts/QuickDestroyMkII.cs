using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Literally all this does is destroy something soon after it's made
/// This version is designed solely for shine
/// TODO: get rid of this script once we get actual attack animations in
/// </summary>
public class QuickDestroyMkII : MonoBehaviour {
	void Start () { Destroy(this.gameObject, GameObject.Find("Player").GetComponent<PlayerCombat>().shActive * Helper.frame); }
}
