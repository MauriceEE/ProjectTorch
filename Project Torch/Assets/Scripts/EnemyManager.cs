using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages all the enemies in the scene
/// Also acts as the encounter manager
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
    //Encounter type
    protected Encounter.SpecialEncounters currentEncounterType;
    //Aggression for the current encounter
    protected float encounterAggressionCurrent;
    //Text manager
    protected TextManager textMan;
    //Wave manager
    protected WaveManager waveMan;
    //Used between waves; the encounter just pauses
    protected bool encounterPaused = false;
    #endregion

    #region Public Fields
    //Enemies in a combat encounter
    public List<GameObject> encounterEnemies;
    //Whether or not a spot is occupied in the grid, in the following format:
    //      3         0
    //    4      P      1
    //      5         2
    public GameObject[] surroundingGridOccupants;
    //Movement speed modifier for allies
    public float allySpeedMultiplier;
    //Max amount of HP enemies can take
    public float encounterAggressionLimit;
    //Amount of aggression lost per second
    public float encounterAggressionLossPerSecond;
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
    public GameObject[] enemyCategories;
    #endregion

    #region Properties
    public List<GameObject> Enemies { get { return zoneEnemies; } }
    public GameObject[] HumanEnemyCategories { get { return humanEnemyCategories; } }
    public GameObject[] ShadowEnemyCategories { get { return shadowEnemyCategories; } }
    public bool EncounterPaused { get { return encounterPaused; } set { encounterPaused = value; } }
    public bool EncounterActive { get { return encounterActive; } set { encounterActive = value; } }
    public float EncounterAggressionCurrent { get { return encounterAggressionCurrent; } set { encounterAggressionCurrent = value; } }
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
                if (e.IsAttacking) 
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
    void Awake () {
        zoneEnemies = new List<GameObject>();
        encounterEnemies = new List<GameObject>();
        gameEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        player = GameObject.Find("Player");
        surroundingGridOccupants = new GameObject[6];
        for (int i = 0; i < 6; ++i) 
            surroundingGridOccupants[i] = null;
        gridPositions = new Vector2[6];
        GenerateGridPositions();
        flags = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
        textMan = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        waveMan = GameObject.Find("WaveManagerGO").GetComponent<WaveManager>();
    }
	
	void Update () {
        CleanupEnemies();
        if (encounterActive)
            ManageEncounter();
    }
    #endregion

    #region Custom Methods
    // N/A
    #endregion

    #region Encounter Methods
    /// <summary>
    /// Sets up an encounter
    /// CALLED BY ENEMIES
    /// </summary>
    public void StartEncounter(Encounter enc, Enemy.EnemyFaction hitEnemyFaction, Encounter.SpecialEncounters encounterType)
    {
        //change music to combat at encounter start
        SoundManager.PlayMusic(SoundManager.Music.Combat, this.gameObject);
        if (encounterActive)
            MergeEncounters();
        Helper.DebugLogDivider();
        //Reset aggression
        encounterAggressionCurrent = 0f;
        //Set flagged for being in an encounter
        encounterActive = true;
        //Set encounter type
        currentEncounterType = encounterType;
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
            }
            if (encounterEnemies.Count > 6)
            {
                Debug.Log("More than 6 enemies in encounter!");
                Debug.Break();
                throw new UnityException();//Don't allow more than 6 enemies in an encounter... if this line of code triggers then we need to fix something
            }
        }

        // spawn any extra enemies
        foreach (int key in enc.categoryXEnemies) SpawnExtraEnemies(key); // spawn any extra enemies

        Debug.Log("Starting Encounter... Enemies = " + encounterEnemies.Count + ", now assigning attack targets... @ " + Time.fixedTime);
        //Now that we have all the encounter enemies setup, we can assign attack targets
        for (int i = 0; i < encounterEnemies.Count; ++i)
        {
            Enemy e = encounterEnemies[i].GetComponent<Enemy>();
            //Tell the enemies not to ally the player if they're of an opposing faction
            if (e.faction == hitEnemyFaction)
            {
                e.AlliedWithPlayer = false;
                //Try to add enemy to grid cell if they're not allied with the player
                e.MoveTarget = SendNewMoveTarget(e);
            }
            else
            {
                e.AlliedWithPlayer = true;
                e.gameObject.GetComponent<Entity>().SpeedModifier *= allySpeedMultiplier;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
                e.AttackTarget = GetNewAttackTarget(e.faction);
            }
        }
    }

    /// <summary>
    /// Handles what should happen if an encounter starts while another one is active
    /// Basically moves all enemies from the previous one into the new one
    /// NOTE: This shouldn't really happen. Pretty much reserved for edge cases (like scripted events)
    /// </summary>
    protected void MergeEncounters()
    {
        Debug.Log("Merging encounters...... FINISH ME");
    }

    /// <summary>
    /// Removes any enemies that have been flagged as not alive
    /// </summary>
    void CleanupEnemies()
    {
        for (int i = 0; i < gameEnemies.Length; ++i)
        {
            if (gameEnemies[i].activeInHierarchy && !gameEnemies[i].GetComponent<Enemy>().Alive)
            {
                //Remove enemy from big list of enemies
                zoneEnemies.Remove(gameEnemies[i]);
                if (currentEncounterType == Encounter.SpecialEncounters.PrincessRescue && zoneEnemies.Count == 0)
                {
                    //Tell the princess that she's saved
                    GameObject.Find("Princess").GetComponent<Princess>().SavePrincess();
                }
                //Remove from list of interactive NPCs
                for (int j = 0; j < textMan.InteractiveNPCs.Length; ++j)
                    if (textMan.InteractiveNPCs[j] == gameEnemies[i])
                        textMan.InteractiveNPCs[j] = null;
                //Remove enemy from list of encounter enemies if it's there
                if (encounterEnemies.Contains(gameEnemies[i]))
                    encounterEnemies.Remove(gameEnemies[i]);
                //Remove enemy from array of enemies surrounding the player if it's there
                for (int j = 0; j < 6; ++j)
                    if (surroundingGridOccupants[j] == gameEnemies[i])
                        surroundingGridOccupants[j] = null;
                //Check to see if any encounter enemies had it as their attack target
                Enemy e;
                for (int j = 0; j < encounterEnemies.Count; ++j)
                {
                    e = encounterEnemies[j].GetComponent<Enemy>();
                    if (e.AttackTarget == gameEnemies[i])
                    {
                        e.AttackTarget = null;
                        e.AttackTarget = GetNewAttackTarget(e.faction);
                    }
                }
                //Destroy gameobject
                //Destroy(gameEnemies[i]);//DEPRECATED -- Now we're just setting to inactive so that the enemy can respawn on death
                //Tell the flag manager whether or not the enemy was a human
                flags.EnemyKilled(gameEnemies[i].GetComponent<Enemy>().faction == Enemy.EnemyFaction.Human);
                //Set to null
                //gameEnemies[i] = null;//Again, only doing inactive stuff now
                gameEnemies[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Spawns any extra enemies that the specified encounter object desires.
    /// </summary>
    /// <param name="enemyKey">Takes a number 0-5. Even numbers are shadow enemies and odd numbers are humans.
    /// 0 and 1 are the basic enemies.
    /// 2 and 3 are the brutes.
    /// 4 and 5 are the special classes
    /// </param>
    public void SpawnExtraEnemies(int enemyKey)
    {
        if (enemyKey < enemyCategories.Length)
        {
            // spawn category enemies
            GameObject newEnemy = Instantiate(enemyCategories[enemyKey], this.transform.position, this.transform.rotation);
            // add enemy to all necessary data structures
            encounterEnemies.Add(newEnemy);
            zoneEnemies.Add(newEnemy);
            gameEnemies = GameObject.FindGameObjectsWithTag("Enemy"); // remake gameEnemies array
                                                                      // set enemy zone
            newEnemy.GetComponent<Enemy>().zone = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>().CurrentZone.zone;
            // start the encounter for that enemy
            newEnemy.GetComponent<Enemy>().StartEncounter();
        }
        else Debug.Log("Enemy key greater than enemyCategory array length. Check the number you input.");
    }

    /// <summary>
    /// Returns an enemy of a certain faction that's in the encounter
    /// </summary>
    /// <param name="allyFaction">Faction of the enemy requesting the target</param>
    /// <returns>An enemy that can be set as the attack target</returns>
    public GameObject GetNewAttackTarget(Enemy.EnemyFaction allyFaction)
    {
        Debug.Log("Finding new ally attack target...");
        int rando = Random.Range(0, encounterEnemies.Count);
        int ogRando = rando;
        while (encounterEnemies[rando].GetComponent<Enemy>().faction == allyFaction)
        {
            //Debug.Log("Forever allies?: " + Time.fixedTime);
            //Look for next enemy
            ++rando;
            //Make sure we're not out of bounds
            if (rando >= encounterEnemies.Count)
                rando = 0;
            //If we looped through all possible other enemies...
            if (rando == ogRando)
            {
                Debug.Log("No more enemies of opposing faction! Can't get new attack target");
                return null;
            }
        }
        return encounterEnemies[rando];
    }

    /// <summary>
    /// Sets all enemies of a certain faction to be aggressive
    /// </summary>
    /// <param name="faction">Enemy faction to set as hostile</param>
    public void SetGlobalAggression(Enemy.EnemyFaction faction)
    {
        for (int i = 0; i < zoneEnemies.Count; ++i)
        {
            Enemy e = zoneEnemies[i].GetComponent<Enemy>();
            if (e.faction == faction)
                e.AlliedWithPlayer = false;
        }
    }

    public bool CanEnemiesAttackPlayer()
    {
        Enemy e;
        // loop through all enemies, checking to see if anyone is currently attacking
        for (int i = 0; i < encounterEnemies.Count; i++)
        {
            e = encounterEnemies[i].GetComponent<Enemy>();
            if (e.IsAttacking && !e.AlliedWithPlayer) return false;
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
            UpdateEnemiesAlliedWithPlayer();
            MoveToAttackPlayer();
            //Reduce current aggression, but keep above zero
            encounterAggressionCurrent -= encounterAggressionLossPerSecond * Time.deltaTime;
            if (encounterAggressionCurrent < 0)
                encounterAggressionCurrent = 0f;
            //Check if aggression is above limit and set aggression if true
            if (encounterAggressionCurrent >= encounterAggressionLimit)
            {
                for (int i = 0; i < encounterEnemies.Count; ++i)
                {
                    encounterEnemies[i].GetComponent<Enemy>().AlliedWithPlayer = false;
                    encounterEnemies[i].GetComponent<Entity>().SpeedModifier *= 1f;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
                }
            }
        }
        else
        {
            //Encounter over, cleanup...
            for (int i = 0; i < encounterEnemies.Count; ++i)
                encounterEnemies[i].GetComponent<Enemy>().EndEncounter();
            encounterEnemies.Clear();
            for (int i = 0; i < 6; ++i)
                surroundingGridOccupants[i] = null;
            //change music to ambient at encounter end
            SoundManager.PlayMusic(SoundManager.Music.Ambient, this.gameObject);
        }
    }
    /// <summary>
    /// This method goes through all the enemies that are allied with the player and updates them
    /// </summary>
    protected void UpdateEnemiesAlliedWithPlayer()
    {
        Enemy e;
        for (int i = 0; i < encounterEnemies.Count; ++i)
        {
            e = encounterEnemies[i].GetComponent<Enemy>();
            if (e.AlliedWithPlayer)
            {
                //Make sure they have a target to attack
                if (e.AttackTarget == null)
                    e.MoveToAttack(GetNewAttackTarget(e.faction));
                else if (e.EnemyState == Enemy.EnemyStates.ReturningFromAttack)//Make them attack again after they attack
                    e.MoveToAttack(e.AttackTarget);
            }
        }
    }

    /// <summary>
    /// Tells an enemy to move in to position and attack
    /// </summary>
    protected void MoveToAttackPlayer()
    {
        //If ready to attack, attack
        if (timeBeforeNextAttack <= 0) 
        {
            if (CanEnemiesAttackPlayer())
            {
                //Make random enemy attack
                //This little bit here finds a random enemy among the ones circling the player in the grid cells
                int randIndex = Random.Range(0, 6);

                /* // check to ensure the grid isn't entirely null, thus triggering an infinite loop
                bool allNull = true;
                for(int x = 0; x < surroundingGridOccupants.Length; x++)
                {
                    if (surroundingGridOccupants[x] != null) allNull = false;
                    else if (x == (surroundingGridOccupants.Length - 1) && allNull == true) Debug.Break();

                    Debug.Log(surroundingGridOccupants[x]);
                }
                */

                while (surroundingGridOccupants[randIndex] == null)
                {
                    //Debug.Log("Infinite loop?: " + Time.fixedTime);
                    //Debug.Log(randIndex);
                    ++randIndex;
                    if (randIndex >= 6)
                        randIndex = 0;
                }
                if (surroundingGridOccupants[randIndex].GetComponent<Enemy>().AlliedWithPlayer)
                {
                    Debug.Log("player ally is attacking the player, fix me plz");
                    //Debug.Break();
                }
                //Tell the enemy we found to attack
                if (surroundingGridOccupants[randIndex].GetComponent<Enemy>().EnemyState != Enemy.EnemyStates.ApproachingToAttack
                    && !surroundingGridOccupants[randIndex].GetComponent<Enemy>().IsAttacking
                    && surroundingGridOccupants[randIndex].GetComponent<Enemy>().EnemyState != Enemy.EnemyStates.Stunned
                    && surroundingGridOccupants[randIndex].GetComponent<Enemy>().EnemyState != Enemy.EnemyStates.Knockback
                    && surroundingGridOccupants[randIndex].GetComponent<Enemy>().EnemyState != Enemy.EnemyStates.Dodging)
                {
                    surroundingGridOccupants[randIndex].GetComponent<Enemy>().MoveToAttack(player);
                    //Remove him from the guys surrounding the player
                    surroundingGridOccupants[randIndex] = null;
                    
                    //Cooldown before next attack order
                    timeBeforeNextAttack = Random.Range(attackMinWait, attackMaxWait);
                }
            }
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
        //But only do these checks if we aren't in the final encounter
        if (currentEncounterType != Encounter.SpecialEncounters.CastleKeepFinalEncounter
            && currentEncounterType != Encounter.SpecialEncounters.ThroneRoomFinalEncounter)
        {
            //Check to see if there are no enemies remaining in the encounter
            if (encounterEnemies.Count == 0)
            {
                float playerHP = player.GetComponent<PlayerCombat>().hp;
                encounterActive = false;
                //Special case: final boss encounter
                if (currentEncounterType == Encounter.SpecialEncounters.ThroneRoomFinalEncounter)
                    GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>().GameOver();
                Debug.Log("No enemies remaining: encounter ending...");
                playerHP += 30;
                if (playerHP > 100) playerHP = 100;
                return;
            }

            //Check to see if there are no enemies of the opposing faction
            bool enemyRemains = false;
            for (int i = 0; i < encounterEnemies.Count; ++i)
            {
                if (!encounterEnemies[i].GetComponent<Enemy>().AlliedWithPlayer)
                {
                    enemyRemains = true;
                    break;
                }
            }
            if (!enemyRemains)
            {
                encounterActive = false;
                Debug.Log("No more enemies of opposing faction, encounter ending...");
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
        else
        {
            //STUFF HERE IS FOR FINAL BOSS ENCOUNTER ENDING
            //Check to see if there are no enemies remaining in the encounter
            if (!encounterPaused && encounterEnemies.Count == 0)
            {
                Debug.Log("CURRENT ENCOUNTER TYPE: " + currentEncounterType);
                //If we're at the final wave...
                if (waveMan.FinalWave(waveMan.SpecialEncounterTypeToWaveID(currentEncounterType)))
                {
                    Debug.Log("Final boss encounter over...");
                    encounterActive = false;
                    //TODO: King in downed state?
                    GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>().GameOver();
                }
                else
                {
                    Debug.Log("Next wave of final boss encounter...");
                    //TODO: Trigger dialogue with the king?
                    //Spawn the next wave after the cooldown
                    waveMan.StartTimerForNextWave(waveMan.SpecialEncounterTypeToWaveID(currentEncounterType));
                    encounterPaused = true;
                }
            }
        }
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
        //FIND A BETTER SOLUTION
        CheckSurroundingGridDuplicates();
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
        Debug.Log("SendMoveTarget method broke... Listing occupants");
        for (int i = 0; i < 6; ++i)
            Debug.Log(surroundingGridOccupants[i]);
        Debug.Break();
        throw new UnityException();//Code shouldn't ever reach here
    }

    /// <summary>
    /// Makes an ally attack an enemy
    /// Should be called in the enemy class when the enemy is allied with the player
    /// </summary>
    /// <param name="e">The enemy script object</param>
    public void SendAllyAttackOrder(Enemy e)
    {
        e.MoveToAttack(GetNewAttackTarget(e.faction));
    }

    /// <summary>
    /// Temporary solution to prevent the game-breaking bug that happens in the SendNewMoveTarget method
    /// TODO: Find the actual source of the problem
    ///     i.e. why enemies are being added to the grid twice
    ///         Perhaps not removed properly?
    ///         Perhaps check to see if they already exist when adding them?
    /// </summary>
    protected void CheckSurroundingGridDuplicates()
    {
        for (int i = 0; i < surroundingGridOccupants.Length; ++i)
        {
            for (int j = i + 1; j < surroundingGridOccupants.Length; ++j) 
            {
                if (surroundingGridOccupants[i] == surroundingGridOccupants[j])
                    surroundingGridOccupants[j] = null;
            }
        }
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
    /// <summary>
    /// Updates enemies in the zoneEnemies list
    /// Should be called when transitioning to a new zone
    /// </summary>
    /// <param name="zone">Current zone the player is in</param>
    public void GetEnemiesInCurrentZone(ZoneManager.ZoneNames zone)
    {
        Enemy e;
        //First wipe out old enemies
        zoneEnemies.Clear();
        //Loop through all enemies to determine whether or not they're in the right zone
        for (int i = 0; i < gameEnemies.Length; ++i)
        {
            //Not sure if there's a more effecient way to do this but
            //THIS ONLY WORKS IF THE FORMAT IN THE HIERARCHY IS 
            //  -Zone-,-ActiveArea-,-Enemies-,-Enemy-
            //So keep it that way when making new levels please

            /*
            if (gameEnemies[i] != null && gameEnemies[i].GetComponent<Enemy>().zone == zone) 
                zoneEnemies.Add(gameEnemies[i]);
                */
            e = gameEnemies[i].GetComponent<Enemy>();
            if (e.zone == zone)
            {
                //Reset the enemy back to normal
                zoneEnemies.Add(gameEnemies[i]);
                gameEnemies[i].SetActive(true);
                e.Alive = true;
                e.hp = e.MaxHP;
                e.transform.position = e.StartingPosition;
                e.EndEncounter();
                e.MoveTarget = e.StartingPosition;
            }
            ///TODO:
            ///Alternate option, try FindObjectsOfType, since it only grabs active objects. Might be more efficient, might not
        }
    }
    #endregion
}