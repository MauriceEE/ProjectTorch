using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages all the enemies in the scene
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
    //Time currently waiting between attacks
    protected float timeBeforeNextAttack;
    //Whether or not currently in an encounter
    protected bool encounterActive = false;
    //Precalculated grid positions
    protected Vector2[] gridPositions;
    //Flag manager
    protected FlagManager flags;
    #endregion

    #region Public Fields
    //Enemies in a combat encounter
    public List<GameObject> encounterEnemies;
    //Whether or not a spot is occupied in the grid, in the following format:
    //      3         0
    //    4      P      1
    //      5         2
    public GameObject[] surroundingGridOccupants;
    //Range of encounters
    public float encounterRadius;
    //Angle of the enemies above/below the player (as opposed to directly in front)
    public float surroundingGridAngle;
    //Size of the invisible grid surrounding the player during encounters
    public float surroundingGridRadius;
    //Size of the circles around which enemies will vary their movements once in the grid
    public float surroundingGridCirclesRadius;
    //Min/Max amount of time to wait to move to a new point in the circle
    public float surroundingGridCirclesMinWait;
    public float surroundingGridCirclesMaxWait;
    //Min/Max amount of time between attacks
    public float attackMinWait;
    public float attackMaxWait;
    //Enemy categories for spawning in encounters
    //Encounter objects will fetch these values when trying to see what kind of enemies to spawn
    public GameObject[] humanEnemyCategories;
    public GameObject[] shadowEnemyCategories;
    #endregion

    #region Properties
    public List<GameObject> Enemies { get { return zoneEnemies; } }
    public GameObject[] HumanEnemyCategories { get { return humanEnemyCategories; } }
    public GameObject[] ShadowEnemyCategories { get { return shadowEnemyCategories; } }
    /// <summary>
    /// Returns a list of all enemy hitboxes that are currently active
    /// Used for the player class when doing shine
    /// TODO: only get enemy faction hitboxes
    /// </summary>
    public List<Rect> EnemyAttackHitboxes { get
        {
            List<Rect> hitboxes = new List<Rect>();
            Enemy e;
            for (int i = 0; i < encounterEnemies.Count; ++i)
            {
                e = encounterEnemies[i].GetComponent<Enemy>();
                if (e.isAttacking) 
                {
                    hitboxes.Add(e.atHB1);
                    hitboxes.Add(e.atHB2);
                    hitboxes.Add(e.atHB3);
                }
            }
            return hitboxes;
        } }
    #endregion

    #region Unity Methods
    void Start () {
        zoneEnemies = new List<GameObject>();
        encounterEnemies = new List<GameObject>();
        gameEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject o in gameEnemies)
            zoneEnemies.Add(o);
        player = GameObject.Find("Player");
        surroundingGridOccupants = new GameObject[6];
        for (int i = 0; i < surroundingGridOccupants.Length; ++i) 
            surroundingGridOccupants[i] = null;
        gridPositions = new Vector2[6];
        GenerateGridPositions();
        flags = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
    }
	
	void Update () {
        CleanupEnemies();
        //MoveEnemies();
        if (encounterActive)
            ManageEncounter();
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
                //Remove enemy from array of enemies surrounding the player if it's there
                for (int j = 0; j < 6; ++j) 
                    if (surroundingGridOccupants[j] == gameEnemies[i])
                        surroundingGridOccupants[j] = null;
                //Destroy gameobject
                Destroy(gameEnemies[i]);
                //Tell the flag manager whether or not the enemy was a human
                flags.EnemyKilled(gameEnemies[i].GetComponent<Enemy>().faction == Enemy.EnemyFaction.Human);
                //Set to null
                gameEnemies[i] = null; 
            }
        }
    }
    #endregion

    #region Encounter Methods
    /// <summary>
    /// Sets up an encounter
    /// CALLED BY ENEMIES
    /// </summary>
    public void StartEncounter(Encounter enc, Enemy.EnemyFaction hitEnemyFaction)
    {
        Helper.DebugLogDivider();
        //Set flagged for being in an encounter
        encounterActive = true;
        //Update position of the encounter (which will simply be used as this object's location for debug purposes)
        this.transform.position = enc.transform.position;
        //Grab enemies within range and add them to the encounter
        for (int i = 0; i < zoneEnemies.Count; ++i) 
        {
            //Try to add enemy to encounter
            if ((enc.transform.position - zoneEnemies[i].transform.position).sqrMagnitude <= Mathf.Pow(enc.Range, 2))
            {
                encounterEnemies.Add(zoneEnemies[i]);
                zoneEnemies[i].GetComponent<Enemy>().StartEncounter();
                //Try to add enemy to grid cell
                Enemy e = encounterEnemies[encounterEnemies.Count - 1].GetComponent<Enemy>();
                e.MoveTarget = SendNewMoveTarget(e);
                //Tell the enemies not to ally the player if they're of an opposing faction
                if (e.faction == hitEnemyFaction)
                    e.AlliedWithPlayer = false;
            }

            if (encounterEnemies.Count > 6)
                throw new UnityException();//Don't allow more than 6 enemies in an encounter... if this line of code triggers then we need to fix something
        }
        Debug.Log("Starting Encounter... Enemies = " + encounterEnemies.Count);
    }

    /// <summary>
    /// Sets all enemies of a certain faction to be aggressive
    /// </summary>
    /// <param name="faction">Enemy faction to set as hostile</param>
    public void SetGlobalAggression(Enemy.EnemyFaction faction)
    {
        for (int i = 0; i < gameEnemies.Length; ++i)
        {
            Enemy e = gameEnemies[i].GetComponent<Enemy>();
            if (e.faction == faction)
                e.AlliedWithPlayer = false;
        }
    }

    public bool CanEnemiesAttack()
    {
        // loop through all enemies, checking to see if anyone is currently attacking
        for (int i = 0; i < encounterEnemies.Count; i++)
        {
            if (encounterEnemies[i].GetComponent<Enemy>().isAttacking == true) return false;
        }
        return true;
    }

    /// <summary>
    /// This method calls other encounter-related subroutines for organization sake
    /// </summary>
    protected void ManageEncounter()
    {
        CheckEncounterEnd();
        if (encounterActive)
        {
            UpdateEnemiesInGrid();
            MoveToAttack();
        }
        else
        {
            //Encounter over, cleanup...
            for (int i = 0; i < encounterEnemies.Count; ++i)
                encounterEnemies[i].GetComponent<Enemy>().EndEncounter();
            encounterEnemies.Clear();
            for (int i = 0; i < 6; ++i)
                surroundingGridOccupants[i] = null;
        }
    }

    /// <summary>
    /// Tells an enemy to move in to position and attack
    /// </summary>
    protected void MoveToAttack()
    {
        //If ready to attack, attack
        if (CanEnemiesAttack() && timeBeforeNextAttack <= 0) 
        {
            //Make random enemy attack
            //This little bit here finds a random enemy among the ones circling the player in the grid cells
            int randIndex = Random.Range(0, 6);
            while (surroundingGridOccupants[randIndex] == null) 
            {
                ++randIndex;
                if (randIndex >= 6)
                    randIndex = 0;
            }
            //Tell the enemy we found to attack
            surroundingGridOccupants[randIndex].GetComponent<Enemy>().MoveToAttack(player);
            //Remove him from the guys surrounding the player
            surroundingGridOccupants[randIndex] = null;
            //Cooldown before next attack order
            timeBeforeNextAttack = Random.Range(attackMinWait, attackMaxWait);
        }
        else
            timeBeforeNextAttack -= Time.deltaTime;
    }

    /// <summary>
    /// Removes a specific enemy from the occupants grid
    /// </summary>
    /// <param name="e">Enemy to remove</param>
    public void RemoveEnemyFromOccupancyGrid(Enemy e)
    {
        for (int i = 0; i < 6; ++i)
            if (surroundingGridOccupants[i] == e.gameObject)
                surroundingGridOccupants[i] = null;
    }

    /// <summary>
    /// Checks end conditions for the encounter
    /// TODO: only check for non-allied NPCs when deciding if there are no enemies left
    /// </summary>
    protected void CheckEncounterEnd()
    {
        //Check to see if there are no enemies remaining in the encounter
        if (encounterEnemies.Count == 0)
        {
            encounterActive = false;
            Debug.Log("No enemies remaining: encounter ending...");
            return;
        }
            
        //Check to see if the player fled by running outside awareness range of all enemies
        bool fled = true;
        for (int i = 0; i < encounterEnemies.Count; ++i)
        {
            if ((encounterEnemies[i].transform.position - player.transform.position).sqrMagnitude < Mathf.Pow(encounterEnemies[i].GetComponent<Enemy>().AwarenessRange, 2)) 
            {
                fled = false;
                break;
            }
        }
        encounterActive = !fled;
        if (!encounterActive)
            Debug.Log("Player fled: encounter ending...");
    }

    /// <summary>
    /// Updates all enemies in the grid around the player
    /// Should be called whenever an encounter is active
    /// </summary>
    protected void UpdateEnemiesInGrid()
    {
        for (int i = 0; i < 6; ++i) 
        {
            if (surroundingGridOccupants[i])
            {
                //Update moveTarget
                surroundingGridOccupants[i].GetComponent<Enemy>().MoveTarget = GeneratePositionInGridCircle(i, surroundingGridOccupants[i].transform.position);
                //Debug.Log("Enemy " + i + " in grid updated...");
            }
        }
    }

    /// <summary>
    /// Gives an enemy a position to move to and assigns the enemy to a grid cell
    /// Then also changes the enemy's state to circle around the player
    /// TODO: Rewrite this to be more efficient if necessary
    /// </summary>
    /// <param name="enemy">Enemy which called this method</param>
    /// <returns>Position for the enemy to move to</returns>
    public Vector3 SendNewMoveTarget(Enemy enemy)
    {
        //If the enemy is already to the left, try to assign them to a left spot
        if (enemy.transform.position.x < player.transform.position.x)
        {
            //Check directly left of the player first 
            if (!surroundingGridOccupants[4]) 
            {
                surroundingGridOccupants[4] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(4, enemy.transform.position);
            }
            //Else try up/down grid cells
            else if (!surroundingGridOccupants[3])
            {
                surroundingGridOccupants[3] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(3, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[5])
            {
                surroundingGridOccupants[5] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(5, enemy.transform.position);
            }
            //IF none of those spots are open, check to the right
            else if (!surroundingGridOccupants[1])
            {
                surroundingGridOccupants[1] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(1, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[0])
            {
                surroundingGridOccupants[0] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(0, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[2])
            {
                surroundingGridOccupants[2] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(2, enemy.transform.position);
            }
        }
        else//If the enemy started out on the right...
        {
            //Try the right first
            if (!surroundingGridOccupants[1])
            {
                surroundingGridOccupants[1] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(1, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[0])
            {
                surroundingGridOccupants[0] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(0, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[2])
            {
                surroundingGridOccupants[2] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(2, enemy.transform.position);
            }
            //THen try the left
            else if (!surroundingGridOccupants[4])
            {
                surroundingGridOccupants[4] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(4, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[3])
            {
                surroundingGridOccupants[3] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(3, enemy.transform.position);
            }
            else if (!surroundingGridOccupants[5])
            {
                surroundingGridOccupants[5] = enemy.gameObject;
                enemy.CircleAroundPlayer();
                return GeneratePositionInGridCircle(5, enemy.transform.position);
            }
        }
        Debug.Log("SendMoveTarget method broke");
        Debug.Break();
        throw new UnityException();//Code shouldn't ever reach here
    }

    /// <summary>
    /// Returns a position on one of the circles in the grid cells surrounding the player
    /// TODO: Find a better random if the enemy's position doesn't work well
    /// </summary>
    /// <param name="index">Index of the grid, refer to private vars above</param>
    /// <param name="enemyPos">Position of the enemy, used for randomness</param>
    /// <returns></returns>
    protected Vector3 GeneratePositionInGridCircle(int index, Vector3 enemyPos)
    {
        return player.transform.position + Helper.Vec2ToVec3(gridPositions[index]) + new Vector3(
            Helper.Map(Mathf.PerlinNoise(enemyPos.x, 0f), 0f, 1f, -1f, 1f) * surroundingGridCirclesRadius, 
            Helper.Map(Mathf.PerlinNoise(0f, enemyPos.y), 0f, 1f, -1f, 1f) * surroundingGridCirclesRadius);
    }

    /// <summary>
    /// Pre-calculates all the grid positions because we're gonna be using them a lot later
    /// </summary>
    protected void GenerateGridPositions()
    {
        gridPositions[0] = new Vector2(surroundingGridRadius * (Mathf.Cos(Mathf.Deg2Rad * surroundingGridAngle)), surroundingGridRadius * (Mathf.Sin(Mathf.Deg2Rad * surroundingGridAngle)));
        gridPositions[1] = new Vector2(surroundingGridRadius, 0);
        gridPositions[2] = new Vector2(surroundingGridRadius * (Mathf.Cos(Mathf.Deg2Rad * surroundingGridAngle)), -(surroundingGridRadius * (Mathf.Sin(Mathf.Deg2Rad * surroundingGridAngle))));
        gridPositions[3] = new Vector2(-(surroundingGridRadius * (Mathf.Cos(Mathf.Deg2Rad * surroundingGridAngle))), surroundingGridRadius * (Mathf.Sin(Mathf.Deg2Rad * surroundingGridAngle)));
        gridPositions[4] = new Vector2(-surroundingGridRadius, 0);
        gridPositions[5] = new Vector2(-(surroundingGridRadius * (Mathf.Cos(Mathf.Deg2Rad * surroundingGridAngle))), -(surroundingGridRadius * (Mathf.Sin(Mathf.Deg2Rad * surroundingGridAngle))));
    }
    #endregion
}