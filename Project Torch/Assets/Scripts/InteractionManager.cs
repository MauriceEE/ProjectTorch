using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles what happens whenever the player presses the interact key
/// </summary>
public class InteractionManager : MonoBehaviour
{
    #region Public Fields
    public float interactRange;
#endregion

    #region Private Fields
    //Reference to inventory manager
    protected Inventory inventory;
    //Reference to dialogue manager
    protected DialogueManager dialogue;
    //Reference to zone manager
    protected ZoneManager zone;
    //Reference to the text manager
    protected TextManager text;
    //Reference to the flag manager
    protected FlagManager flags;
    //Reference to the player
    protected GameObject player;
    #endregion

    #region Unity Defaults
    void Start()
    {
        inventory = GameObject.Find("InventoryManagerGO").GetComponent<Inventory>();
        dialogue = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        zone = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>();
        text = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        player = GameObject.Find("Player");
        flags = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
    }

    void Update()
    {
        //Check to see if they're trying to interact
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            //Try to pick up an item
            inventory.PickUpNearbyItems();
            //Try to talk to NPCs
            CheckInteractiveNPCs();
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// This method loops through all the interactable NPCs, 
    /// and if within range it will tell the flag manager to play dialogue lines
    /// </summary>
    protected void CheckInteractiveNPCs()
    {
        //Loop through all interactive NPCs
        for (int i = 0; i < text.InteractiveNPCs.Length; ++i) 
        {
            //Only check active NPCs, and check to see if they're within interaction range
            if (text.InteractiveNPCs[i].gameObject.activeSelf && (text.InteractiveNPCs[i].transform.position - player.transform.position).sqrMagnitude <= Mathf.Pow(interactRange, 2f)) 
            {
                //Tell the flag manager to look through the flags to determine what line of dialogue to use
                flags.ActivateNPCDialogue(text.InteractiveNPCs[i].npcID);
                return; //Only try to interact with 1 NPC 
            }
        }
    }
#endregion
}