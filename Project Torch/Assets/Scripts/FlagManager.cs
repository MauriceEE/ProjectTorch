using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script deals with flags and keeps track of them
/// </summary>
public class FlagManager : MonoBehaviour {

    #region Enums
    // Note: this list is taken directly from the Story "Paths" doc on the drive
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
    #endregion

    #region Public Fields
    //Number of enemy kills to set a faction hostile
    public int killsToTiggerHostility;
#endregion

    #region Private Fields
    //Dictionary of all flags and a boolean to determine whether or not they're true
    protected Dictionary<FlagNames, bool> flags;
    //Reference to dialogue manager
    protected DialogueManager dialogue;
    //Reference to text manager
    protected TextManager text;
    //Number of enemies killed
    protected int humansKilled, shadowsKilled;
    #endregion

    #region Properties
    public Dictionary<FlagNames,bool> FlagList { get { return flags; } set { flags = value; } }
#endregion

    #region Unity Defaults
    void Start () {
        dialogue = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        text = GameObject.Find("TextManagerGO").GetComponent<TextManager>();
        flags = new Dictionary<FlagNames, bool>();
        //Battlefield
        flags.Add(FlagNames.AllMustPerish, false);
        //Sullen Village
        flags.Add(FlagNames.BanditAttack, false);
        flags.Add(FlagNames.InjuredBandits, false);
        flags.Add(FlagNames.FrightenedVillagers, false);
        flags.Add(FlagNames.WorriedMother, false);
        flags.Add(FlagNames.VillageDefender, false);
        //The Castle
        flags.Add(FlagNames.ClosedGate, false);
        flags.Add(FlagNames.AssaultingTheGate, false);
        flags.Add(FlagNames.GateDefender, false);
        flags.Add(FlagNames.DefaultKingOfManDialogue, true);
        flags.Add(FlagNames.HostileKingOfMan, false);
        flags.Add(FlagNames.DeathOfMansHope, false);
        flags.Add(FlagNames.FreeThePrincess, false);
        flags.Add(FlagNames.KingOfMansBlessing, false);
        flags.Add(FlagNames.BeginTheRitual, false);
        //Dark Village
        flags.Add(FlagNames.MissingChild, false);
        flags.Add(FlagNames.MissingPendant, false);
        flags.Add(FlagNames.ReturningThePendant, false);
        //Fortress of dark
        flags.Add(FlagNames.StalwartSentinels, false);
        flags.Add(FlagNames.SecretOfTheSentinels, false);
        flags.Add(FlagNames.WrathfulKing, false);
        flags.Add(FlagNames.CaptainsPlan, false);
        flags.Add(FlagNames.DeathOfShadow, false);
        //Misc
        flags.Add(FlagNames.EnemyOfMan, false);
        flags.Add(FlagNames.EnemyOfShadow, false);
        flags.Add(FlagNames.DeathOfTheCaptain, false);
        flags.Add(FlagNames.FearOfFlame, false);
    }
    #endregion

    #region Custom Methods
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
                if (flags[FlagNames.EnemyOfShadow])
                    dialogue.AddDialogueSequence(text.Lines["King of the Dark - Downed"]);
                break;
            case TextManager.InteractiveNPCNames.CaptainOfTheGuard:
                break;
        }
    }
    /// <summary>
    /// Adds to the total number of enemies killed and updates "EnemyOf" flags
    /// </summary>
    /// <param name="human"></param>
    public void EnemyKilled(bool human)
    {
        if (human)
            flags[FlagNames.EnemyOfMan] = (++humansKilled > killsToTiggerHostility);
        else
            flags[FlagNames.EnemyOfShadow] = (++shadowsKilled > killsToTiggerHostility);
    }
#endregion
}
