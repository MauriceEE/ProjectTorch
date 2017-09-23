using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    //List of all enemies... used for other classes
    protected List<GameObject> enemies;
    public List<GameObject> Enemies { get { return enemies; } }
    //Array of enemies gathered from the FindGameObjectsWithTag function
    protected GameObject[] enemiesArray;

	void Start () {
        enemies = new List<GameObject>();
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in enemiesArray)
            enemies.Add(o);
	}
	
	void Update () {
        CleanupEnemies();
	}

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
}
