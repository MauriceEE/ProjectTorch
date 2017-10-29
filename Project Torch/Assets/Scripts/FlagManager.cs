﻿using System.Collections;
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

        //REWORKED FLAGS
        PrincessRescue,
        BattlefieldBrazierLit,
        HumanTerritory1BrazierLit,
        ShadowTerritory1BrazierLit,
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
    public int HumansKilled { get { return humansKilled; } }
    public int ShadowsKilled { get { return shadowsKilled; } }
    #endregion

    #region Unity Defaults
    void Awake () {
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

        //REWORK
        flags.Add(FlagNames.PrincessRescue, false);
        flags.Add(FlagNames.BattlefieldBrazierLit, false);
        flags.Add(FlagNames.HumanTerritory1BrazierLit, true);
        flags.Add(FlagNames.ShadowTerritory1BrazierLit, false);
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
    /// Also tells the enemy manager to set global aggression if the hostility limit is reached
    /// </summary>
    /// <param name="human"></param>
    public void EnemyKilled(bool human)
    {
        if (human)
            ++humansKilled;
        else
            ++shadowsKilled;
        //Check "EnemyOf" flags
        if (!flags[FlagNames.EnemyOfMan])
        {
            if (humansKilled > killsToTiggerHostility)
            {
                flags[FlagNames.EnemyOfMan] = true;
                GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().SetGlobalAggression(Enemy.EnemyFaction.Human);
            }
        }
        if (!flags[FlagNames.EnemyOfShadow])
        {
            if (shadowsKilled > killsToTiggerHostility)
            {
                flags[FlagNames.EnemyOfShadow] = true;
                GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().SetGlobalAggression(Enemy.EnemyFaction.Shadow);
            }
        }
    }
    /// <summary>
    /// Returns whether or not the conditions for the Princess Rescue stage or met
    /// Called by ZoneManager
    /// </summary>
    /// <returns>True if conditions met</returns>
    public bool PrincessRescue()
    {
        Debug.Log(flags[FlagNames.BattlefieldBrazierLit] + " " + flags[FlagNames.ShadowTerritory1BrazierLit]);
        return (flags[FlagNames.BattlefieldBrazierLit] &&
            flags[FlagNames.ShadowTerritory1BrazierLit] &&
            humansKilled >= 5 &&
            shadowsKilled < 3);
    }
    /// <summary>
    /// Flags whether or not a brazier is lit
    /// Should be called whenever a breazier is manipulated
    /// </summary>
    /// <param name="zone">Zone the brazier is in</param>
    /// <param name="lit">Whether or not it's lit</param>
    public void BrazierLit(ZoneManager.ZoneNames zone, bool lit)
    {
        switch (zone)
        {
            case ZoneManager.ZoneNames.Battlefield:
                flags[FlagNames.BattlefieldBrazierLit] = lit;
                break;
            case ZoneManager.ZoneNames.HumanTerritoryStage1:
                flags[FlagNames.HumanTerritory1BrazierLit] = lit;
                break;
            case ZoneManager.ZoneNames.ShadowTerritoryStage1:
                flags[FlagNames.ShadowTerritory1BrazierLit] = lit;
                break;
        }
    }
#endregion
}
