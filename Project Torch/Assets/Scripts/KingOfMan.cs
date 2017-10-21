using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingOfMan : MonoBehaviour {

    //GameObject dManager = GameObject.Find("DialogueManager");
    DialogueManager dManager;

    // Use this for initialization
    void Start () {
        dManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.E))
        {
            //dManager.AddDialogueSequence(dManager.tManager.Lines ["King of Man - Default"]);

            //Debug.Log(dManager.tManager); //tManager is coming out as null when called like this
        }

    }
}
