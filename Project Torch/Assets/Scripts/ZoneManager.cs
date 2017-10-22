using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
/// <summary>
/// Handles zones/zone transition
/// </summary>
public class ZoneManager : MonoBehaviour {
    #region Enums
    //All possible zones for the player to be in
    public enum Zones
    {
        Battlefield,
        SullenVillage,
        ThrivingVillage,
        CastleOfMan,
        FortressOfDark
    }
    //Used to keep track of how to manipulate the black screen obj
    protected enum TransitionPhase
    {
        NONE,
        FadingOut,
        FadingIn
    }
    #endregion

    #region Public Fields
    //Current zone the player is in
    public Zones currentZone;
    //Reference to the black screen object for scene transitions
    public GameObject blackScreen;
    //Reference to all the zone transition objects
    [Header("End Points")]
    public GameObject battlefieldLeftEndPoint;
    public GameObject battlefieldRightEndPoint;
    public GameObject sullenVillageLeftEndPoint;
    public GameObject sullenVillageRightEndPoint;
    public GameObject thrivingVillageLeftEndPoint;
    public GameObject thrivingVillageRightEndPoint;
    public GameObject throneRoomEndPoint;
    public GameObject fortressKeepEndPoint;
    //Reference to all the parent zone objects
    [Header("Zone Parents")]
    public GameObject battlefieldZone;
    public GameObject sullenVillageZone;
    public GameObject thrivingVillageZone;
    public GameObject throneRoomZone;
    public GameObject fortressKeepZone;
    #endregion

    #region Private Fields
    //Reference to the camera's post process changer
    protected PostProcessChange profileChanger;
    //The material of the black screen, used for fading
    protected Material screenFade;
    //The current phase of transitioning
    protected TransitionPhase phase;
    //The player
    protected GameObject player;
    //Camera clamp
    protected float cameraMin, cameraMax;
    //Stuff to transition the player
    protected Zones nextZone;
    protected float newPlayerXCoordinate;
    #endregion

    #region Unity Defaults
    void Start () {
        profileChanger = Camera.main.GetComponent<PostProcessChange>();
        screenFade = blackScreen.GetComponent<Renderer>().material;
        player = GameObject.Find("Player");
        //Assign camera clamp values
        UpdateCameraClamp();
        //Make black screen transparent at first
        blackScreen.SetActive(true);
        Color fadeColor = screenFade.color;
        fadeColor.a = 0f;
        screenFade.color = fadeColor;
    }
	
	void Update () {
        //Set darkness to true if in the thriving village or fortress of dark
        profileChanger.darkness = (currentZone == Zones.ThrivingVillage || currentZone == Zones.FortressOfDark);
        //Keep the camera within the bounds
        ClampCamera();
        //Check to see if within range of a zone end
        switch(phase)
        {
            case TransitionPhase.NONE:
                switch (currentZone)
                {
                    case Zones.Battlefield:
                        if (player.transform.position.x <= battlefieldLeftEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = battlefieldLeftEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        else if (player.transform.position.x >= battlefieldRightEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = battlefieldRightEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        break;
                    case Zones.SullenVillage:
                        if (player.transform.position.x <= sullenVillageLeftEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = sullenVillageLeftEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        else if (player.transform.position.x >= sullenVillageRightEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = sullenVillageRightEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        break;
                    case Zones.ThrivingVillage:
                        if (player.transform.position.x <= thrivingVillageLeftEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = thrivingVillageLeftEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        else if (player.transform.position.x >= thrivingVillageRightEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = thrivingVillageRightEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        break;
                    case Zones.CastleOfMan:
                        if (player.transform.position.x <= throneRoomEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = throneRoomEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        break;
                    case Zones.FortressOfDark:
                        if (player.transform.position.x <= fortressKeepEndPoint.transform.position.x)
                        {
                            ZoneEndPoint endpoint = fortressKeepEndPoint.GetComponent<ZoneEndPoint>();
                            ChangeZone(endpoint.nextZone, endpoint.newXCoordinate);
                        }
                        break;
                }
                break;
            case TransitionPhase.FadingIn:
                FadeIn();
                break;
            case TransitionPhase.FadingOut:
                FadeOut();
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
    protected void ChangeZone(Zones _nextZone, float newXCoordinate)
    {
        phase = TransitionPhase.FadingOut;
        nextZone = _nextZone;
        newPlayerXCoordinate = newXCoordinate;
    }
    /// <summary>
    /// Locks the camera within a bounds, based on the zone
    /// </summary>
    protected void ClampCamera()
    {
        if (Camera.main.transform.position.x < cameraMin)
            Camera.main.transform.position = new Vector3(cameraMin, Camera.main.transform.position.y, Camera.main.transform.position.z);
        else if (Camera.main.transform.position.x > cameraMax)
            Camera.main.transform.position = new Vector3(cameraMax, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
    /// <summary>
    /// Updates where the camera should be clamped to
    /// </summary>
    protected void UpdateCameraClamp()
    {
        switch (currentZone)
        {
            case Zones.Battlefield:
                cameraMin = battlefieldLeftEndPoint.transform.position.x;
                cameraMax = battlefieldRightEndPoint.transform.position.x;
                break;
            case Zones.SullenVillage:
                cameraMin = sullenVillageLeftEndPoint.transform.position.x;
                cameraMax = sullenVillageRightEndPoint.transform.position.x;
                break;
            case Zones.ThrivingVillage:
                cameraMin = thrivingVillageLeftEndPoint.transform.position.x;
                cameraMax = thrivingVillageRightEndPoint.transform.position.x;
                break;
            case Zones.CastleOfMan:
                cameraMin = 0f;
                cameraMax = 0f;
                break;
            case Zones.FortressOfDark:
                cameraMin = 0f;
                cameraMax = 0f;
                break;
        }
    }
    /// <summary>
    /// Activates/deactivates the zone game objects
    /// </summary>
    /// <param name="zone">Zone to manipulate</param>
    /// <param name="active">Whether it should be active or inactive</param>
    protected void SetZoneActive(Zones zone, bool active)
    {
        switch (zone)
        {
            case Zones.Battlefield:
                battlefieldZone.SetActive(active);
                break;
            case Zones.SullenVillage:
                sullenVillageZone.SetActive(active);
                break;
            case Zones.ThrivingVillage:
                thrivingVillageZone.SetActive(active);
                break;
            case Zones.CastleOfMan:
                throneRoomZone.SetActive(active);
                break;
            case Zones.FortressOfDark:
                fortressKeepZone.SetActive(active);
                break;
        }
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
            //Deactivate this zone
            SetZoneActive(currentZone, false);
            //Activate the next one
            SetZoneActive(nextZone, true);
            //Update current zone
            currentZone = nextZone;
            //Update the camera min/max
            UpdateCameraClamp();
            //Update player's position
            player.transform.position = new Vector3(newPlayerXCoordinate, player.transform.position.y, player.transform.position.z);
            //Move to fade in phase
            phase = TransitionPhase.FadingIn;
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
#endregion
}