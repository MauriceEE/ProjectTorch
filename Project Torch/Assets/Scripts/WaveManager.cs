using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Handles waves and spawns them
/// Spawn calls are done by the enemy manager though
/// Note that the WaveIDs enum has to be set up properly
///     The SpecialEncounterTypeToWaveID method at the bottom helps out 
///     converting between that and special encounter types though
/// </summary>
public class WaveManager : MonoBehaviour {
#region Enums
    public enum WaveIDs
    {
        NONE,
        KingOfManFinalBoss,
        KingOfDarkFinalBoss,
    }
    #endregion
#region Public Fields
    public Wave[] kingOfManFinalBossWaves;
    public Wave[] kingOfDarkFinalBossWaves;
    public float timeBetweenWaves;
    #endregion
#region Private Fields
    protected int currentWaveKingOfMan;
    protected int currentWaveKingOfDark;
    protected bool runningTimer;
    protected float elapsedTime;
    protected WaveIDs currentWaveID;
    protected EnemyManager enemyMan;
    #endregion
#region Unity Defaults
    void Start () {
        currentWaveKingOfMan = 0;
        currentWaveKingOfDark = 0;
        runningTimer = false;
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
	}
	
	void Update () {
        if (runningTimer)
        {
            //Increment time
            elapsedTime += Time.deltaTime;
            //if we're past the limit, spawn the next wave
            if (elapsedTime >= timeBetweenWaves)
            {
                //turn off the timer
                runningTimer = false;
                //Spawn the next wave
                SpawnNextWave(currentWaveID);
            }
        }
	}
    #endregion
#region Custom Methods
    /// <summary>
    /// Starts a timer to spawn the next wave
    /// Can be skipped by calling SpawnNextWave
    /// </summary>
    /// <param name="waveID"></param>
    public void StartTimerForNextWave(WaveIDs waveID)
    {
        //Reset time between waves
        timeBetweenWaves = 0f;
        //Flag the timer to start going
        runningTimer = true;
        //Specify current wave ID
        currentWaveID = waveID;
    }

    /// <summary>
    /// Tells the current wave to spawn itself and then gives the contained enemies to the enemy manager
    /// </summary>
    /// <param name="wave">Wave ID to spawn</param>
    public void SpawnNextWave(WaveIDs wave)
    {
        GameObject[] waveEnemies = null;
        switch (wave)
        {
            case WaveIDs.KingOfDarkFinalBoss:
                //Get wave enemies, store them temporarily, and increment current wave
                waveEnemies = kingOfDarkFinalBossWaves[currentWaveKingOfDark].SpawnEnemies();
                ++currentWaveKingOfDark;
                break;
            case WaveIDs.KingOfManFinalBoss:
                waveEnemies = kingOfManFinalBossWaves[currentWaveKingOfMan].SpawnEnemies();
                ++currentWaveKingOfMan;
                break;
            case WaveIDs.NONE:
                throw new UnityException("ERROR: Told wave of ID 'NONE' to spawn!");
        }
        if (waveEnemies == null)
            throw new UnityException("ERROR: waveEnemies is null, could not spawn the next wave");
        //Now we need to tell the enemy manager what enemies to add to the encounter
        for (int i = 0; i < waveEnemies.Length; ++i)
        {
            Enemy e = waveEnemies[i].GetComponent<Enemy>();
            //Also make sure they aren't allied with the player
            e.AlliedWithPlayer = false;
            //Tell them to start the encounter
            e.StartEncounter();
            //Tell them to attack the player and add them to the circle
            e.MoveTarget = enemyMan.SendNewMoveTarget(e);
            //Actually go and add them to the encoutner itself
            enemyMan.encounterEnemies.Add(waveEnemies[i]);
        }
        //Resume the encounter
        enemyMan.EncounterPaused = false;
    }

    /// <summary>
    /// Returns whether or not this is the final wave of a series of waves
    /// </summary>
    /// <param name="waveID">ID of the set of waves</param>
    /// <returns>True if it's the last one</returns>
    public bool FinalWave(WaveIDs waveID)
    {
        switch (currentWaveID)
        {
            case WaveIDs.KingOfDarkFinalBoss:
                return currentWaveKingOfDark >= kingOfDarkFinalBossWaves.Length;
            case WaveIDs.KingOfManFinalBoss:
                return currentWaveKingOfMan >= kingOfManFinalBossWaves.Length;
        }
        return true;
    }

    /// <summary>
    /// Converts a special encounter type enum to a wave ID enum
    /// </summary>
    /// <param name="encType">Special encounter type</param>
    /// <returns>Wave ID</returns>
    public WaveIDs SpecialEncounterTypeToWaveID(Encounter.SpecialEncounters encType)
    {
        WaveIDs result = WaveIDs.NONE;
        switch (encType)
        {
            case Encounter.SpecialEncounters.CastleKeepFinalEncounter:
                result = WaveIDs.KingOfDarkFinalBoss;
                break;
            case Encounter.SpecialEncounters.ThroneRoomFinalEncounter:
                result = WaveIDs.KingOfManFinalBoss;
                break;
            default:
                throw new UnityException("ERROR: Bad encounter type input!");
        }
        return result;
    }
    #endregion
}
