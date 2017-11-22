using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour {

    public Text instructionBar;
    private Dictionary<string, string> instructions;
    private string currentInstruction;
	// Use this for initialization
	void Start ()
    {
        instructions = new Dictionary<string, string>();
        instructions.Add("Off", "");
        instructions.Add("Move", "WASD  to  move  and  [L]  to  evade  attacks");
        instructions.Add("Slash", "Press  [J]  to  deal  damage  with  Slash");
        instructions.Add("Thrust", "Press  [K]  to  break  yellow  guards  with  Thrust");
        instructions.Add("Shine", "Press  [I]  when  enemies  are  purple  to  counter  attacks  with  Shine");
        instructions.Add("Brazier", "Press  [E] on  braziers  if  you  wish  to  light  the  next  level");
    }
	
	// Update is called once per frame
	void Update () {
        instructionBar.text = currentInstruction;
	}

    /// <summary>
    /// Changes the instructions in the HUD
    /// </summary>
    /// <param name="newInstruction">Look at dictionary in InstructionManager to see what keys map to which instructions. "Off" is nothing</param>
    public void changeInstructions(string newInstruction)
    {
        currentInstruction = instructions[newInstruction];
    }
}
