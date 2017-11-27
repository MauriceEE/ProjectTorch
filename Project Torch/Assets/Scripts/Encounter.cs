using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
/// <summary>
/// Manages a single encounter object
/// </summary>
public class Encounter : MonoBehaviour {
    #region Enums
    public enum SpecialEncounters
    {
        NONE,
        PrincessRescue,
        ThroneRoomFinalEncounter,
        CastleKeepFinalEncounter,
    }
    #endregion

    #region Public Fields
    public SpecialEncounters specialEncounterType;
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
    public float Range { get { return range; } set { range = value; } }
    #endregion

    #region Unity Defaults
    void Awake () {
        active = false;
        //First we gotta make sure that the enemies flagged to be assigned are actually assigned
        for (int i = 0; i < triggerEnemies.Count; ++i)
            triggerEnemies[i].GetComponent<Enemy>().Encounter = this.gameObject;
	}
    #endregion

    #region Custom Methods
    /// <summary>
    /// Simply starts an encounter
    /// </summary>
    public void StartEncounter(Enemy.EnemyFaction hitEnemyFaction)
    {
        active = true;
        GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().StartEncounter(this, hitEnemyFaction, specialEncounterType);
    }
#endregion
}