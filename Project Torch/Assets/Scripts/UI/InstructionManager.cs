using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour {

    public Text instructionBar;
    private Dictionary<string, string> instructions;
    private string currentInstruction;
    private string currentKey;
    private PlayerCombat player;
    private float timeElapsed;
    // Use this for initialization
    void Start ()
    {
        instructions = new Dictionary<string, string>();
        instructions.Add("Off", "");
        instructions.Add("Move", "WASD  to  move  and  [L]  to  evade  attacks");
        instructions.Add("Slash", "Press  [J]  to  deal  damage  with  Slash");
        instructions.Add("Thrust", "Press  [K]  to  break  yellow  guards  with  Thrust.  Brutes  regenerate  guard");
        instructions.Add("Shine", "Press  [I]  to  slow  and  light  enemies");
        instructions.Add("Shine2", "Shine  when  an  enemy  flashes  purple  to attack  to  stun  them");
        instructions.Add("Brazier", "Press  [E] on  braziers  if  you  wish  to  light  the  next  level");
        instructions.Add("Darkness", "Use  Shine  in  combat  to  manage  visibility.");
        instructions.Add("Pulse", "Press  [Spacebar]  to  extend  light  from  lit  enemies  to  unlit  enemies");
        instructions.Add("Brute", "Brutes put up yellow guards after some time. Hit them while you can!");
        instructions.Add("Glower", "Glowers shoot projectiles that explode and slow you!");
        instructions.Add("GlowerReflect", "Hit their projectile with Thrust to reflect it");
        instructions.Add("Spearman", "Quick, long-reaching Spearmen dodge after every attack");

        currentKey = "Off";
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        timeElapsed = 0;
    }
	
	// Update is called once per frame
	void Update () {
        instructionBar.text = currentInstruction;

        // update elapsed time
        if (timeElapsed > 0) timeElapsed += Time.deltaTime;

        switch (currentKey)
        {
            case "Off":
                break;

            case "Shine":
                if (player.CurrentAttack == PlayerCombat.Attacks.Shine) startTime();
                
                if (timeElapsed > 3)
                {
                    timeElapsed = 0;
                    changeInstructions("Shine2");
                }
                break;

            case "Darkness":
                if (player.CurrentAttack == PlayerCombat.Attacks.Shine) startTime();

                if (timeElapsed > 3)
                {
                    timeElapsed = 0;
                    changeInstructions("Pulse");
                }
                break;

            case "Glower":
                startTime();

                if (timeElapsed > 4)
                {
                    timeElapsed = 0;
                    //changeInstructions("Pulse");
                }
                break;
        }
	}

    /// <summary>
    /// Changes the instructions in the HUD
    /// </summary>
    /// <param name="newInstruction">Look at dictionary in InstructionManager to see what keys map to which instructions. "Off" is nothing</param>
    public void changeInstructions(string newInstruction)
    {
        currentKey = newInstruction;
        currentInstruction = instructions[newInstruction];
        timeElapsed = 0;
    }

    private void startTime()
    {
        if(timeElapsed == 0) timeElapsed += Time.deltaTime;
    }
}
