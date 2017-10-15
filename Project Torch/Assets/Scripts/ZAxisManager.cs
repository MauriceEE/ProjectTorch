using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Version 2.0, now updated to never touch the Z axis
/// No more complaining about Z axis lighting shenanigans!
/// </summary>
public class ZAxisManager : MonoBehaviour {
    #region Public Fields
    public int minZ, maxZ;
    public float minY, maxY;
    public GameObject[] objs;
    #endregion
    #region Private Fields
    protected EnemyManager enemyMan;
    #endregion
    #region Properties
    public float MinY { get { return minY; } }
    public float MaxY { get { return maxY; } }
    #endregion
    #region Unity Methods
    void Start () {
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
	}
	void Update () {
        for (int i = 0; i < objs.Length; ++i)
            UpdateZAxis(objs[i]);
        for (int i = 0; i < enemyMan.Enemies.Count; ++i)
            UpdateZAxis(enemyMan.Enemies[i]);
	}
    #endregion
    #region Custom Methods
    void UpdateZAxis(GameObject o) {
        o.GetComponent<SpriteRenderer>().sortingOrder = (int)Helper.Map(o.transform.position.y, minY, maxY, minZ, maxZ);
        //o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, );//This is the old method
    }
#endregion
}
