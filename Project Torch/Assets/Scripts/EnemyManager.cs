using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    protected List<GameObject> enemies;
    public List<GameObject> Enemies { get { return enemies; } }

    protected GameObject[] enemiesArray;

	void Start () {
        enemies = new List<GameObject>();
        //Object[] etemp = FindObjectsOfType<Enemy>();
        //enemiesArray = new GameObject[etemp.Length];
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        //for (int i = 0; i < etemp.Length; ++i)
        //    enemiesArray[i] = etemp[i] as GameObject;
        foreach (GameObject o in enemiesArray)
            enemies.Add(o);
	}
	
	void Update () {
        CleanupEnemies();
	}

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
