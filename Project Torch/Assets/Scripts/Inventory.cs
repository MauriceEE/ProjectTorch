using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

#region Public Fields
    //Range at which the player can grab items
    public float playerPickupRange;
    //Total number of items holdable
    public int maxItems;
    #endregion
#region Private Fields
    //Items currently held by the player
    protected Item[] heldItems;
    //All possible items in level
    protected Item[] itemsInLevel;
    //Reference to player object
    protected GameObject player;
    //Effectively count of heldItems array
    protected int numHeldItems;
    #endregion
#region Unity Defaults
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
    #endregion
#region Custom Methods
    /// <summary>
    /// If there's an item nearby, it will be destroyed and then added to inventory
    /// </summary>
    public void PickUpNearbyItems()
    {
        //Make sure they don't have too many items already
        if (numHeldItems < maxItems)
        {
            Vector2 playerPos, itemPos;
            playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            //Loop through all items in level
            for (int i = 0; i < itemsInLevel.Length; ++i)
            {
                if(itemsInLevel[i]!=null)
                {
                    itemPos = new Vector2(itemsInLevel[i].gameObject.transform.position.x, itemsInLevel[i].gameObject.transform.position.y);
                    //Check to see if they're within pickup range
                    if ((itemPos - playerPos).sqrMagnitude < playerPickupRange * playerPickupRange)
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
#endregion
}
