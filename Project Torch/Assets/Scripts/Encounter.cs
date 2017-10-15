using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Manages a single encounter object
/// </summary>
public class Encounter : MonoBehaviour {

#region Public Fields
    public List<GameObject> triggerEnemies;
    public SpawnedEnemy[] spawnedEnemies;
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
    public void StartEncounter()
    {
        active = true;
        EnemyManager enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        enemyMan.StartEncounter(this);
    }
#endregion
}

#region Classes
/// <summary>
/// Merely holds vars needed to parameterize an enemy
/// TODO: Flesh this out with necessary values
/// </summary>
[System.Serializable]
public class SpawnedEnemy
{
    public int HP;
    public Vector2 moveSpeed;
    public bool test;
}
/// <summary>
/// This little bit here draws lines and arcs visually showing the encounter range
/// </summary>
[CustomEditor(typeof(Encounter))] 
public class DrawWireArcEncounter : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Encounter enc = (Encounter)target;
        Handles.DrawWireArc(enc.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, enc.Range);
        Handles.DrawDottedLine(enc.transform.position, enc.transform.position + new Vector3(0, enc.Range, 0), 12f);
        Handles.DrawDottedLine(enc.transform.position, enc.transform.position + new Vector3(enc.Range, 0, 0), 12f);
    }
}
#endregion