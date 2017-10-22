using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script deals with flags and keeps track of them
/// </summary>
public class FlagManager : MonoBehaviour {

    /// <summary>
    /// Note: this list is taken directly from the Story "Paths" doc on the drive
    /// </summary>
    public enum FlagNames
    {
        //Battlefield
        AllMustPerish,
        //Sullen village
        BanditAttack,
        InjuredBandits,
        FrightenedVillagers,
        WorriedMother,
        VillageDefender,
        //The castle
        ClosedGate,
        AssaultingTheGate,
        GateDefender,
        DefaultKingOfManDialogue,
        HostileKingOfMan,
        DeathOfMansHope,
        FreeThePrincess,
        KingOfMansBlessing,
        BeginTheRitual,
        //Dark village
        MissingChild,
        MissingPendant,
        ReturningThePendant,
        //Fortress of dark
        StalwartSentinels,
        SecretOfTheSentinels,
        WrathfulKing,
        CaptainsPlan,
        DeathOfShadow,
        //Misc
        EnemyOfMan,
        EnemyOfShadow,
        DeathOfTheCaptain,
        FearOfFlame,
    }

    //Dictionary of all flags and a boolean to determine whether or not they're true
    protected Dictionary<FlagNames, bool> flags;
    //Reference to dialogue manager
    protected DialogueManager dialogue;
    //Reference to text manager
    protected TextManager text;

    void Start () {
        dialogue = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        text = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        flags = new Dictionary<FlagNames, bool>();
        flags.Add(FlagNames.AllMustPerish, false);
        flags.Add(FlagNames.BanditAttack, false);
        flags.Add(FlagNames.InjuredBandits, false);
        flags.Add(FlagNames.FrightenedVillagers, false);
        flags.Add(FlagNames.WorriedMother, false);
        flags.Add(FlagNames.VillageDefender, false);
        flags.Add(FlagNames.ClosedGate, false);
        flags.Add(FlagNames.AssaultingTheGate, false);
        flags.Add(FlagNames.GateDefender, false);
        flags.Add(FlagNames.DefaultKingOfManDialogue, true);
        flags.Add(FlagNames.HostileKingOfMan, false);
        flags.Add(FlagNames.DeathOfMansHope, false);
        flags.Add(FlagNames.FreeThePrincess, false);
        flags.Add(FlagNames.KingOfMansBlessing, false);
        flags.Add(FlagNames.BeginTheRitual, false);
        flags.Add(FlagNames.MissingChild, false);
        flags.Add(FlagNames.MissingPendant, false);
        flags.Add(FlagNames.ReturningThePendant, false);
        flags.Add(FlagNames.StalwartSentinels, false);
        flags.Add(FlagNames.SecretOfTheSentinels, false);
        flags.Add(FlagNames.WrathfulKing, false);
        flags.Add(FlagNames.CaptainsPlan, false);
        flags.Add(FlagNames.DeathOfShadow, false);
        flags.Add(FlagNames.EnemyOfMan, false);
        flags.Add(FlagNames.EnemyOfShadow, false);
        flags.Add(FlagNames.DeathOfTheCaptain, false);
        flags.Add(FlagNames.FearOfFlame, false);
    }
	
	void Update () {
        
	}

    /// <summary>
    /// This function will make an NPC talk based on their current flags
    /// Gets called by the interaction manager
    /// </summary>
    /// <param name="npcName">Name of the NPC to check flags for</param>
    public void ActivateNPCDialogue(TextManager.InteractiveNPCNames npcName)
    {
        switch (npcName)
        {
            case TextManager.InteractiveNPCNames.KingOfMan:
                if (flags[FlagNames.DefaultKingOfManDialogue]) 
                    dialogue.AddDialogueSequence(text.Lines["King of Man - Default"]);
                break;
            case TextManager.InteractiveNPCNames.KingOfDark:
                break;
            case TextManager.InteractiveNPCNames.CaptainOfTheGuard:
                break;
        }
    }
}
