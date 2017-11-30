using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
/// <summary>
/// Handles zones/zone transition
/// </summary>
public class ZoneManager : MonoBehaviour {
    #region Enums
    //All possible zones for the player to be in
    public enum ZoneNames
    {
        Battlefield,
        FortressKeep,
        HumanTerritoryStage1,
        HumanTerritoryStage2,
        PrincessRescue,
        ShadowTerritoryStage1,
        ShadowTerritoryStage2,
        ThroneRoom,
        WarZone,
        WarZoneStage2,
        TrueHumanStage1,
    }
    //Used to keep track of how to manipulate the black screen obj
    protected enum TransitionPhase
    {
        NONE,
        FadingOut,
        FadingIn,
        FadingToGameOver,
    }
    #endregion

    #region Public Fields
    //Zone the game will begin at
    public ZoneNames startingZone;
    //Reference to the black screen object for scene transitions
    public GameObject blackScreen;
    #endregion

    #region Private Fields
    //Reference to the camera's post process changer
    protected PostProcessChange profileChanger;
    //Reference to enemy manager
    protected EnemyManager enemyMan;
    //The material of the black screen, used for fading
    protected Material screenFade;
    //The current phase of transitioning
    protected TransitionPhase phase;
    //The player
    protected GameObject player;
    //Camera clamp
    protected float cameraMax;
    //Stuff to transition the player
    protected Zone nextZone;
    //Sorted dictionary of zones for easy lookup
    protected Dictionary<ZoneNames, Zone> zonesSorted;
    //The current zone the player is in
    protected Zone currentZone;
    //Flag manager
    protected FlagManager flagMan;
    //Array of all possible zones in the game
    protected Zone[] zones;
    protected InstructionManager instructMan;
    #endregion

    #region Properties
    public Zone CurrentZone { get { return currentZone; } }
    public Dictionary<ZoneNames,Zone> ZonesSorted { get { return zonesSorted; } }
    #endregion

    #region Unity Defaults
    void Awake () {
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        profileChanger = Camera.main.GetComponent<PostProcessChange>();
        screenFade = blackScreen.GetComponent<Renderer>().material;
        player = GameObject.Find("Player");
        //Make black screen transparent at first
        blackScreen.SetActive(true);
        Color fadeColor = screenFade.color;
        fadeColor.a = 0f;
        screenFade.color = fadeColor;
        profileChanger.darkness = false;
        //Get all zone scripts on gameobjects
        zones = GameObject.FindObjectsOfType(typeof(Zone)) as Zone[];
        //Sort by zone name for easy access/lookup
        zonesSorted = new Dictionary<ZoneNames, Zone>();
        Zone z;
        for (int i = 0; i < zones.Length; ++i)
        {
            z = zones[i].GetComponent<Zone>();
            zonesSorted.Add(z.zone, z);
        }
        //Set current zone
        currentZone = zonesSorted[startingZone];
        //Assign camera clamp values
        UpdateCameraClamp();
        //Get flag manager
        flagMan = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
        instructMan = GameObject.Find("InstructionManagerGO").GetComponent<InstructionManager>();
    }

    void Start()
    {
        //Deactivate all background zones the player won't start at
        foreach(Zone z in zonesSorted.Values)
        {
            if (z.zone != startingZone)
                z.gameObject.SetActive(false);
        }
        //Update enemy manager's zone enemies
        enemyMan.GetEnemiesInCurrentZone(currentZone.zone);
    }

    void Update () {
        //Keep the camera within the bounds
        ClampCamera();
        //Check to see if within range of a zone end
        switch(phase)
        {
            case TransitionPhase.NONE:
                //Check if player is past the endpoint
                if (player.transform.position.x >= currentZone.endPoint.transform.position.x)
                {
                    //ChangeZone(currentZone.endPoint.GetComponent<ZoneEndPoint>().nextZone); //Start transition to next zone
                    ChangeZone(GetNextZone());
                }
                break;
            case TransitionPhase.FadingIn:
                FadeIn();
                break;
            case TransitionPhase.FadingOut:
                FadeOut();
                break;
            case TransitionPhase.FadingToGameOver:
                FadeToGameOver();
                break;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Executes fade out to a different zone
    /// </summary>
    /// <param name="_nextZone">The next zone to move to</param>
    /// <param name="newXCoordinate">Where to move the player to</param>
    public void ChangeZone(Zone _nextZone)
    {
        phase = TransitionPhase.FadingOut;
        nextZone = _nextZone;
        instructMan.changeInstructions("Off"); // turn off any lingering tutorials
        //newPlayerXCoordinate = newXCoordinate;
    }
    /// <summary>
    /// Locks the camera within a bounds, based on the zone
    /// </summary>
    protected void ClampCamera()
    {
        if (Camera.main.transform.position.x < 0f)
            Camera.main.transform.position = new Vector3(0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        else if (Camera.main.transform.position.x > cameraMax)
            Camera.main.transform.position = new Vector3(cameraMax, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
    /// <summary>
    /// Updates where the camera should be clamped to
    /// </summary>
    protected void UpdateCameraClamp()
    {
        cameraMax = currentZone.endPoint.transform.position.x;
    }
    /// <summary>
    /// Activates/deactivates the zone game objects
    /// </summary>
    /// <param name="zone">Zone (script) to manipulate</param>
    /// <param name="active">Whether it should be active or inactive</param>
    protected void SetZoneActive(Zone zone, bool active)
    {
        zone.gameObject.SetActive(active);
    }
    /// <summary>
    /// Fades out into a black screen and then transitions to a new level
    /// </summary>
    protected void FadeOut()
    {
        if (screenFade.color.a < 1)
        {
            Color fadeColor = screenFade.color;
            fadeColor.a += Time.deltaTime;
            screenFade.color = fadeColor;
        }
        else
        {
            //If we get here, the screen is completely black, so...

            // if there is a brazier active in the world/level
            if (GameObject.FindGameObjectWithTag("Brazier") != null)
            {
                //Set darkness to opposite of brazier lit status
                profileChanger.darkness = !GameObject.FindGameObjectWithTag("Brazier").GetComponent<Brazier>().lit;
            }
            else profileChanger.darkness = false; // if no brazier, next level will be lit
            
            // special cases
            switch(nextZone.zone)
            {
                case ZoneManager.ZoneNames.FortressKeep:
                    profileChanger.darkness = true;
                    break;
            }

            //Deactivate this zone
            SetZoneActive(currentZone, false);
            //Activate the next one
            SetZoneActive(nextZone, true);
            //Update current zone
            currentZone = nextZone;
            //Update the camera min/max
            UpdateCameraClamp();
            //Update player's position
            player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
            //Move to fade in phase
            phase = TransitionPhase.FadingIn;

            //Update zone enemies
            enemyMan.GetEnemiesInCurrentZone(currentZone.zone);
        }
    }
    /// <summary>
    /// Fades the black screen back in
    /// </summary>
    protected void FadeIn()
    {
        if (screenFade.color.a > 0)
        {
            Color fadeColor = screenFade.color;
            fadeColor.a -= Time.deltaTime;
            screenFade.color = fadeColor;
        }
        else phase = TransitionPhase.NONE;
    }
    /// <summary>
    /// Determines the next zone to move the player to and returns it
    /// TODO: Figure out this algorithm based on flags
    /// </summary>
    /// <returns>Next zone the player should go to</returns>
    public Zone GetNextZone()
    {
        //Princess Rescue
        if (currentZone.zone == ZoneNames.ShadowTerritoryStage1)
        {
            if (flagMan.PrincessRescue())
                return zonesSorted[ZoneNames.PrincessRescue];
            else
                return zonesSorted[ZoneNames.ShadowTerritoryStage2];
        }

        //War Zone and True Human Stage 1
        if (currentZone.zone == ZoneNames.Battlefield)
        {
            if (flagMan.WarZone())
                return zonesSorted[ZoneNames.WarZone];
            else if (flagMan.TrueHumanStage1())
                return zonesSorted[ZoneNames.TrueHumanStage1];
            else
                return zonesSorted[ZoneNames.ShadowTerritoryStage1];
        }

        if (currentZone.zone == ZoneNames.ShadowTerritoryStage2 || currentZone.zone == ZoneNames.PrincessRescue) 
            return zonesSorted[ZoneNames.FortressKeep];

        if (currentZone.zone == ZoneNames.FortressKeep)
            return zonesSorted[ZoneNames.HumanTerritoryStage1];

        if (currentZone.zone == ZoneNames.HumanTerritoryStage1)
            return zonesSorted[ZoneNames.HumanTerritoryStage2];

        if (currentZone.zone == ZoneNames.HumanTerritoryStage2)
            return zonesSorted[ZoneNames.ThroneRoom];

        if (currentZone.zone == ZoneNames.WarZone)
            return zonesSorted[ZoneNames.WarZoneStage2];

        if (currentZone.zone == ZoneNames.WarZoneStage2)
            return zonesSorted[ZoneNames.FortressKeep]; //Temporary, will change later

        if (currentZone.zone == ZoneNames.TrueHumanStage1)
            return zonesSorted[ZoneNames.HumanTerritoryStage2]; //Temporary, will change later

        //Code shouldn't get here
        Debug.Log("ZoneManager GetNextZone broke");
        Debug.Break();
        throw new UnityException();
    }
    /// <summary>
    /// Fades out and ends the game
    /// </summary>
    public void GameOver()
    {
        this.phase = TransitionPhase.FadingToGameOver;
    }
    

    /// <summary>
    /// Fades to black and loads the game over scene
    /// </summary>
    protected void FadeToGameOver()
    {
        if (screenFade.color.a < 1)
        {
            Color fadeColor = screenFade.color;
            fadeColor.a += Time.deltaTime;
            screenFade.color = fadeColor;
        }
        else
            SceneManager.LoadScene("GameOver");
    }
#endregion
}