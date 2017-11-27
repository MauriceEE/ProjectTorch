using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The wave object. Holds data for the wave manager to use
/// IMPORTANT NOTE:
///     There must be as many Enemies as there are SpawnSides,
///     so that the wave manager knows where to spawn the enemies
/// </summary>
public class Wave : MonoBehaviour {
#region Enums
    public enum SpawnSide
    {
        Left,
        Right,
    }
    #endregion
#region Public Fields
    public GameObject[] enemies;
    public SpawnSide[] spawnSides;
    #endregion
#region Unity Defaults
    void Awake () {
        //Make sure there are as many sides as enemies
        if (enemies.Length > spawnSides.Length)
            throw new UnityException("ERROR: Need to assign a spawn side for each enemy in the wave!");
	}

    private void Start()
    {
        foreach (GameObject g in enemies)
            g.GetComponent<Enemy>().ignoreAxisConstraints = true;
    }
    #endregion
#region Custom Methods
    /// <summary>
    /// Spawns in the enemies stored in this wave object
    /// </summary>
    /// <returns>The enemies stored here, so that the enemy manager can add them to the encounter</returns>
    public GameObject[] SpawnEnemies()
    {
        //Get min and max Y values
        float minY = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>().CurrentZone.minY;
        float maxY = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>().CurrentZone.maxY;
        //Loop through all enemies
        for (int i = 0; i < enemies.Length; ++i) 
        {
            //enemies[i].SetActive(true);
            //Move to encounter bounds (right outside the camera, camera range is ~10 so I rounded up to 11)
            //Change depending on specified side
            if (spawnSides[i] == SpawnSide.Left)
                enemies[i].transform.position = new Vector3(Camera.main.transform.position.x - 11, Random.Range(minY, maxY), this.transform.position.z);
            else
                enemies[i].transform.position = new Vector3(Camera.main.transform.position.x + 11, Random.Range(minY, maxY), this.transform.position.z);
            //Make sure they actually get constrained to the axis from now on
            enemies[i].GetComponent<Enemy>().ignoreAxisConstraints = false;
        }
        //Give the enemies to the enemy manager so it can add them to the encounter
        return enemies;
    }
    #endregion
}
