using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionTrigger : MonoBehaviour {

    public string textTrigger = "";
    public GameObject leftBound;
    public GameObject rightBound;

    private PlayerCombat player;
    private InstructionManager instructMan;
    private Vector3 triggerPosition;
    private Vector3 leftBoundPosition;
    private Vector3 rightBoundPosition;
    private Rect triggerBoxRect;
    private Rect playerBox;
    private bool seen;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        instructMan = GameObject.Find("InstructionManagerGO").GetComponent<InstructionManager>();
        triggerPosition = transform.TransformVector(this.transform.position);
        leftBoundPosition = transform.TransformVector(leftBound.transform.position);
        rightBoundPosition = transform.TransformVector(rightBound.transform.position);
        seen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (seen == false && player.transform.position.x > leftBoundPosition.x) Trigger();
        if (seen == true && player.transform.position.x > rightBoundPosition.x)
        {
            //instructMan.changeInstructions("Off"); // display nothing after exiting the trigger range
            //seen = false;
        }
    }

    void Trigger()
    {
        seen = true;
        instructMan.changeInstructions(textTrigger); // originally worked via tag but I didn't want to flood the tag dropdown with all of the tutorials we may need
        //Debug.Log("Trigger hit");
    }
}
