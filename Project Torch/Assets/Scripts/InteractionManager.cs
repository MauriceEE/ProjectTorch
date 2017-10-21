using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{

    //Reference to inventory manager
    private Inventory inventory;
    //Reference to dialogue manager
    private DialogueManager dialogue;
    //Reference to zone manager
    private ZoneManager zone;

    private TextManager text;

    // Use this for initialization
    void Start()
    {
        inventory = GameObject.Find("InventoryManagerGO").GetComponent<Inventory>();
        dialogue = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        zone = GameObject.Find("ZoneManagerGO").GetComponent<ZoneManager>();
        text = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if they're trying to interact
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            //Try to pick up an item
            inventory.PickUpNearbyItems();
            dialogue.AddDialogueSequence(text.Lines["King of Man - Default"]);
        }
    }
}