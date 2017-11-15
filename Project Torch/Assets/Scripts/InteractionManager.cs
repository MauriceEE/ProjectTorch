using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles what happens whenever the player presses the interact key
/// </summary>
public class InteractionManager : MonoBehaviour
{
    #region Public Fields
    //Range at which the player can interact with things
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
    //Braziers
    protected Brazier[] braziers;
    //To avoid triggering dialogue multiple times
    protected bool dialogueActive;
    #endregion
    #region Properties
    public bool DialogueActive { get { return dialogueActive; } set { dialogueActive = value; } }
    #endregion
    #region Unity Defaults
    void Awake()
    {
        inventory = GameObject.Find("InventoryManagerGO").GetComponent<Inventory>();
        dialogue = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        zone = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>();
        text = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        player = GameObject.Find("Player");
        flags = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
        braziers = GameObject.FindObjectsOfType(typeof(Brazier)) as Brazier[];
        dialogueActive = false;
        //braziers = new Brazier[brazierObjs.Length];
        //for (int i = 0; i < brazierObjs.Length; ++i)
            //braziers[i] = brazierObjs[i].GetComponent<Brazier>();
    }

    void Update()
    {
        //Check to see if they're trying to interact
        if (!dialogueActive && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))) 
        {
            //Try to pick up an item
            inventory.PickUpNearbyItems();
            //Try to interact with NPCs, Braziers...
            if (CheckInteractiveNPCs() || CheckBraziers())
            {
                dialogueActive = true;
                return;
            }
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// This method loops through all the interactable NPCs, 
    /// and if within range it will tell the flag manager to play dialogue lines
    /// Returns true if dialogue will be displayed
    /// </summary>
    protected bool CheckInteractiveNPCs()
    {
        //Loop through all interactive NPCs
        for (int i = 0; i < text.InteractiveNPCs.Length; ++i) 
        {
            //Only check active NPCs, and check to see if they're within interaction range
            if (text.InteractiveNPCs[i] != null && text.InteractiveNPCs[i].gameObject.activeInHierarchy && (text.InteractiveNPCs[i].transform.position - player.transform.position).sqrMagnitude <= Mathf.Pow(interactRange, 2f)) 
            {
                //Tell the flag manager to look through the flags to determine what line of dialogue to use
                flags.ActivateNPCDialogue(text.InteractiveNPCs[i].npcID);
                return true; //Dialogue active
            }
        }
        return false;
    }
    /// <summary>
    /// Checks interaction with the braziers
    /// Returns true if dialogue will be displayed
    /// </summary>
    protected bool CheckBraziers()
    {
        //Loop through braziers
        for (int i = 0; i < braziers.Length; ++i) 
        {
            //Check to see if you're within range (and in the same level)
            if (braziers[i].gameObject.activeInHierarchy && (braziers[i].transform.position - player.transform.position).sqrMagnitude <= Mathf.Pow(interactRange, 2f))
            {
                //Make shadow people hate you
                flags.FlagList[FlagManager.FlagNames.EnemyOfShadow] = true;
                //Light up the brazier
                string result = braziers[i].IgniteBrazier();
                if (result != null)
                {
                    flags.ActivateDialogueLines(result);
                    return true;
                }
            }
        }
        return false;
    }
#endregion
}