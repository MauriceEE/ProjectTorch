using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAxisManager : MonoBehaviour {

    public float minZ, maxZ;
    public float minY, maxY;
    public GameObject[] objs;

    protected EnemyManager enemyMan;

    public float MinY { get { return minY; } }
    public float MaxY { get { return maxY; } }

	void Start () {
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
	}
	
	void Update () {
        for (int i = 0; i < objs.Length; ++i)
            UpdateZAxis(objs[i]);
        for (int i = 0; i < enemyMan.Enemies.Count; ++i)
            UpdateZAxis(enemyMan.Enemies[i]);
	}

    void UpdateZAxis(GameObject o) {
        o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, Helper.Map(o.transform.position.y, minY, maxY, minZ, maxZ));
    }
}
