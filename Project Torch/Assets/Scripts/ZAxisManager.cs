using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisManager : MonoBehaviour {

    public float minZ, maxZ;
    public float minY, maxY;

    //Reference to the player
    public GameObject player;
    public GameObject[] objs;

    public float MinY { get { return minY; } }
    public float MaxY { get { return maxY; } }

	void Start () {
        player = GameObject.Find("Player");
	}
	
	void Update () {
        UpdateZAxis(player);
        foreach(GameObject g in objs)
        {
            UpdateZAxis(g);
        }
	}

    void UpdateZAxis(GameObject o)
    {
        o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, Mathf.Lerp(minZ, maxZ, Mathf.Lerp(minY, maxY, o.transform.position.y)));
    }
}
