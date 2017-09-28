using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages all the enemies in the scene
/// TODO: only manage enemies in the current level
/// TODO: everything AI related
/// 
/// Stuff to know:
///     Right now, it only gathers all enemies in the scene and checks whether or not to destroy them on Update
/// </summary>
public class EnemyManager : MonoBehaviour {
    #region Private Fields
    //List of all enemies... used for other classes
    protected List<GameObject> enemies;
    //Array of enemies gathered from the FindGameObjectsWithTag function
    protected GameObject[] enemiesArray;
    #endregion
    #region Public Fields
    // N/A
    #endregion
    #region Properties
    public List<GameObject> Enemies { get { return enemies; } }
    #endregion
    #region Unity Methods
    void Start () {
        enemies = new List<GameObject>();
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in enemiesArray)
            enemies.Add(o);
	}
	
	void Update () {
        CleanupEnemies();
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Removes any enemies that have been flagged as not alive
    /// </summary>
    void CleanupEnemies()
    {
        for(int i=0; i< enemiesArray.Length; ++i)
        {
            if (enemiesArray[i] != null && !enemiesArray[i].GetComponent<Enemy>().Alive)
            {
                //Remove enemy from big list of enemies
                enemies.Remove(enemiesArray[i]);
                //Destroy gameobject
                Destroy(enemiesArray[i]);
                //Set to null
                enemiesArray[i] = null; 
            }
        }
    }
    #endregion
}