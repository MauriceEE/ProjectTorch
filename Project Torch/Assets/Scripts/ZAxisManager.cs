using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// At present, this class merely makes sure the Z of all moving objects are correct so that they display properly with depth
/// </summary>
public class ZAxisManager : MonoBehaviour {
    #region Public Fields
    public float minZ, maxZ;
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
        o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y, Helper.Map(o.transform.position.y, minY, maxY, minZ, maxZ));
    }
#endregion
}
