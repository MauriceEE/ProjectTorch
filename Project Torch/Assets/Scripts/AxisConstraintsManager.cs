using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Version 2.0, now updated to never touch the Z axis
/// No more complaining about Z axis lighting shenanigans!
/// </summary>
public class AxisConstraintsManager : MonoBehaviour {
    #region Public Fields
    public int minZ, maxZ;
    #endregion
    #region Private Fields
    protected GameObject[] objectsToMove;
    protected GameObject player;
    protected EnemyManager enemyMan;
    protected ZoneManager zoneMan;
    #endregion
    #region Unity Methods
    void Awake () {
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        zoneMan = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>();
        //Grab the player
        player = GameObject.Find("Player");
        //Grab all braziers
        Brazier[] braziers = FindObjectsOfType(typeof(Brazier)) as Brazier[];
        //After grabbing all objects necessary, add them to the array
        //First we gotta determine the proper size
        //Right now just braziers, modify size accordingly if you add things later
        objectsToMove = new GameObject[braziers.Length];
        //Add braziers
        for (int i = 0; i < braziers.Length; ++i)
            objectsToMove[i] = braziers[i].gameObject;
    }
	void Update () {
        //Player
        UpdateZAxis(player);
        KeepWithinBounds(player);
        //Objects
        for (int i = 0; i < objectsToMove.Length; ++i)
        {
            if (objectsToMove[i].activeInHierarchy)
            {
                UpdateZAxis(objectsToMove[i]);
                KeepWithinBounds(objectsToMove[i]);
            }
        }
        //Enemies
        for (int i = 0; i < enemyMan.Enemies.Count; ++i)
        {
            UpdateZAxis(enemyMan.Enemies[i]);
            KeepWithinBounds(enemyMan.Enemies[i]);
        }
	}
    #endregion
    #region Custom Methods
    protected void UpdateZAxis(GameObject o) {
        o.GetComponent<SpriteRenderer>().sortingOrder = (int)Helper.Map(o.transform.position.y, zoneMan.CurrentZone.minY, zoneMan.CurrentZone.maxY, minZ, maxZ);
    }
    protected void KeepWithinBounds(GameObject g)
    {
        if (g.transform.position.x < zoneMan.CurrentZone.minX)
            g.transform.position = new Vector3(zoneMan.CurrentZone.minX, g.transform.position.y, g.transform.position.z);
        if (g.transform.position.x > zoneMan.CurrentZone.maxX)
            g.transform.position = new Vector3(zoneMan.CurrentZone.maxX, g.transform.position.y, g.transform.position.z);
        if (g.transform.position.y < zoneMan.CurrentZone.minY)
            g.transform.position = new Vector3(g.transform.position.x, zoneMan.CurrentZone.minY, g.transform.position.z);
        if (g.transform.position.y > zoneMan.CurrentZone.maxY)
            g.transform.position = new Vector3(g.transform.position.x, zoneMan.CurrentZone.maxY, g.transform.position.z);
    }
    #endregion
}
