using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    //Range at which the player can grab items
    public float playerPickupRange;
    //Total number of items holdable
    public int maxItems;

    //Items currently held by the player
    protected Item[] heldItems;
    //All possible items in level
    protected Item[] itemsInLevel;
    //Reference to player object
    protected GameObject player;
    //Effectively count of heldItems array
    protected int numHeldItems;

	void Start () {
        numHeldItems = 0;
        heldItems = new Item[maxItems];
        player = GameObject.Find("Player");
        //Find all items in the level and add them to the list
        GameObject[] itemObjs = GameObject.FindGameObjectsWithTag("Item");
        itemsInLevel = new Item[itemObjs.Length];
        for (int i=0; i<itemObjs.Length; ++i)
            itemsInLevel[i] = itemObjs[i].GetComponent<Item>();
	}
	
	void Update () {
        PickUpNearbyItems();
	}

    /// <summary>
    /// If there's an item nearby, it will be destroyed and then added to inventory
    /// </summary>
    protected void PickUpNearbyItems()
    {
        //Make sure they don't have too many items already
        if (numHeldItems < maxItems)
        {
            //Check to see if they're trying to pick up an item
            if (Input.GetKeyDown(KeyCode.E))//CHANGE TO CONTROLLER INPUT LATER
            {
                //Loop through all items in level
                for (int i = 0; i < itemsInLevel.Length; ++i)
                {
                    //Check to see if they're within pickup range
                    if (itemsInLevel[i] != null && (itemsInLevel[i].gameObject.transform.position - player.transform.position).sqrMagnitude < playerPickupRange * playerPickupRange) 
                    {
                        heldItems[numHeldItems] = itemsInLevel[i];//Add item to inventory
                        Destroy(itemsInLevel[i].gameObject);//Remove item from world
                        itemsInLevel[i] = null;//Item can't be picked up again
                        ++numHeldItems;//Increase number of held items
                        return; //Don't try to keep picking up items
                    }
                }
            }
        }
        else
        {
            //SHOW ERROR UNABLE TO PICK UP ITEMS
        }
    }
}
