using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
/// <summary>
/// Manages a single encounter object
/// </summary>
public class Encounter : MonoBehaviour {

    #region Public Fields
    public List<GameObject> triggerEnemies;
    //public SpawnedEnemy[] spawnedEnemies;
    //Number of enemies in category x (array number) to spawn
    public int[] categoryXEnemies;
    public float range;
    #endregion

    #region Private Fields
    protected bool active;
#endregion

    #region Properties
    public List<GameObject> TriggerEnemies { get { return triggerEnemies; } }
    public float Range { get { return range; } }
    #endregion

    #region Unity Defaults
    void Start () {
        active = false;
        //First we gotta make sure that the enemies flagged to be assigned are actually assigned
        for (int i = 0; i < triggerEnemies.Count; ++i)
            triggerEnemies[i].GetComponent<Enemy>().Encounter = this.gameObject;
	}
	
	void Update () {
		
	}
    #endregion

    #region Custom Methods
    /// <summary>
    /// Simply starts an encounter
    /// </summary>
    public void StartEncounter()
    {
        active = true;
        EnemyManager enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        enemyMan.StartEncounter(this);
    }
#endregion
}