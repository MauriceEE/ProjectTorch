using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour {

    public enum FlagNames
    {
        ///ADD FLAGS HERE
        bob,joe,john
    }

    private FlagNames name;
    private Dictionary<FlagNames, bool> flags;

	// Use this for initialization
	void Start () {
        name = FlagNames.bob;
        flags = new Dictionary<FlagNames, bool>();
        flags.Add(FlagNames.bob, true);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(flags[name]);
	}
}
