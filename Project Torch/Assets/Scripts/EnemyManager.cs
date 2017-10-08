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
    //List of all enemies in the current level... used for other classes
    protected List<GameObject> zoneEnemies;
    //Initial array of enemies gathered from the FindGameObjectsWithTag function
    protected GameObject[] gameEnemies;
    //Reference to the player
    protected GameObject player;
    //Enemies in a combat encounter
    protected List<GameObject> encounterEnemies;
    //Whether or not a spot is occupied in the grid, in the following format:
    //      3         0
    //    4      P      1
    //      4         3
    protected bool[] surroundingGridOccupancy;
    //Time currently waiting between attacks
    protected float timeBeforeNextAttack;
    #endregion
    #region Public Fields
    //Range of encounters
    public float encounterRadius;
    //Size of the invisible grid surrounding the player during encounters
    public Vector2 surroundingGridSize;
    //Size of the circles around which enemies will vary their movements once in the grid
    public float surroundingGridCirclesSize;
    //Min/Max amount of time to wait to move to a new point in the circle
    public float surroundingGridCirclesMinWait;
    public float surroundingGridCirclesMaxWait;
    //Min/Max amount of time between attacks
    public float attackMinWait;
    public float attackMaxWait;
    #endregion
    #region Properties
    public List<GameObject> Enemies { get { return zoneEnemies; } }
    #endregion
    #region Unity Methods
    void Start () {
        zoneEnemies = new List<GameObject>();
        encounterEnemies = new List<GameObject>();
        gameEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in gameEnemies)
            zoneEnemies.Add(o);
        player = GameObject.Find("Player");
        surroundingGridOccupancy = new bool[6];
        ResetGridOccupancy();
	}
	
	void Update () {
        CleanupEnemies();
        MoveEnemies();
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Removes any enemies that have been flagged as not alive
    /// </summary>
    void CleanupEnemies()
    {
        for(int i=0; i< gameEnemies.Length; ++i)
        {
            if (gameEnemies[i] != null && !gameEnemies[i].GetComponent<Enemy>().Alive)
            {
                //Remove enemy from big list of enemies
                zoneEnemies.Remove(gameEnemies[i]);
                //Remove enemy from list of encounter enemies if it's there
                if (encounterEnemies.Contains(gameEnemies[i]))
                    encounterEnemies.Remove(gameEnemies[i]);
                //Destroy gameobject
                Destroy(gameEnemies[i]);
                //Set to null
                gameEnemies[i] = null; 
            }
        }
    }

    void MoveEnemies()
    {
        Enemy e;
        foreach(GameObject g in zoneEnemies)
        {
            e = g.GetComponent<Enemy>();
            e.MoveTarget = this.transform.position;
            e.SeekTarget();
        }
    }
    #endregion
#region Encounter Methods
    /// <summary>
    /// Sets up an encounter
    /// CALLED BY ENEMIES
    /// </summary>
    /// <param name="location">Location of the enemy that was hit - encounter centers around there</param>
    public void StartEncounter(Vector3 location)
    {
        //Update position of the encounter (which will simply be used as this object's location)
        this.transform.position = location;
        //Grab enemies within range and add them to the encounter
        foreach (GameObject g in zoneEnemies)
        {
            if ((location - g.transform.position).sqrMagnitude <= Mathf.Pow(encounterRadius, 2))
            {
                encounterEnemies.Add(g);
                g.GetComponent<Enemy>().StartEncounter();
            }
        }
    }

    protected void EndEncounter()
    {
        ResetGridOccupancy();
    }

    /// <summary>
    /// This method exists purely to call other encounter-related subroutines for organization sake
    /// </summary>
    protected void ManageEncounter()
    {

    }


    protected void AssignEnemiesToGrid()
    {
        //////////////
    }

    protected void ResetGridOccupancy()
    {
        for (int i = 0; i < 6; ++i)
            surroundingGridOccupancy[i] = false;
    }

    protected void MoveToAttack(Enemy e)
    {
        //IF ready to attack, attack
        if (timeBeforeNextAttack <= 0) 
        {


            timeBeforeNextAttack = Random.Range(attackMinWait, attackMaxWait);
        }
        else
            timeBeforeNextAttack -= Time.deltaTime;
    }
    #endregion
}