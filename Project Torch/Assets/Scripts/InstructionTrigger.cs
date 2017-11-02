using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour {

    public string textTrigger = "";

    private PlayerCombat player;
    private InstructionManager instructMan;
    private Rect triggerBoxRect;
    private Rect playerBox;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        instructMan = GameObject.Find("InstructionManagerGO").GetComponent<InstructionManager>();
        triggerBoxRect = new Rect(transform.position.x, transform.position.y, GetComponent<BoxCollider2D>().size.x, GetComponent<BoxCollider2D>().size.y);
        playerBox = new Rect(player.transform.position.x, player.transform.position.y, .1f, .1f);
    }
	
	// Update is called once per frame
	void Update () {
        playerBox = new Rect(player.transform.position.x, player.transform.position.y, .1f, .1f);
        if (Helper.AABB(triggerBoxRect, playerBox))
        {
            Trigger();
        };
	}

    void Trigger()
    {
        instructMan.changeInstructions(textTrigger); // originally worked via tag but I didn't want to flood the tag dropdown with all of the tutorials we may need
        //Debug.Log("Trigger hit");
    }
}
