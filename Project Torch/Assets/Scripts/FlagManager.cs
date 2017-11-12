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

        //REWORKED FLAGS
        PrincessRescue,
        BattlefieldBrazierLit,
        HumanTerritory1BrazierLit,
        ShadowTerritory1BrazierLit,
        InteractedWithKingOfMan
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
        flags.Add(FlagNames.InteractedWithKingOfMan, false);
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// This function will make an NPC talk based on their current flags
    /// Gets called by the interaction manager
    /// -Connor Menard
    /// </summary>
    /// <param name="npcName">Name of the NPC to check flags for</param>
    public void ActivateNPCDialogue(TextManager.InteractiveNPCNames npcName)
    {
        switch (npcName)
        {
            case TextManager.InteractiveNPCNames.KingOfMan:
                if (flags[FlagNames.DefaultKingOfManDialogue])
                {
                    ActivateDialogueLines("King of Man - Default");
                    flags[FlagNames.InteractedWithKingOfMan] = true;
                }
                break;
            case TextManager.InteractiveNPCNames.KingOfDark:
                if (flags[FlagNames.EnemyOfShadow])
                {
                    ActivateDialogueLines("King of the Dark - Downed");
                }
                break;
            case TextManager.InteractiveNPCNames.CaptainOfTheGuard:
                break;
        }
    }
    /// <summary>
    /// Invokes any arbitrary line of dialogue, based on the name parameter
    /// -Connor Menard
    /// </summary>
    /// <param name="name">Name of the NPC and its dialogue line</param>
    public void ActivateDialogueLines(string name)
    {
        dialogue.AddDialogueSequence(text.Lines[name], name);
        dialogue.SetTextAndShowImmediately();
    }
    /// <summary>
    /// Adds to the total number of enemies killed and updates "EnemyOf" flags
    /// Also tells the enemy manager to set global aggression if the hostility limit is reached
    /// -Connor Menard
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
    /// Returns whether or not the conditions for the Princess Rescue stage are met
    /// Called by ZoneManager
    /// -Connor Menard
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
    /// Returns whether or not the conditions for the War Zone stage are met
    /// Called by ZoneManager
    /// </summary>
    /// <returns>True if conditions met</returns>
    public bool WarZone()
    {
        //Debug.Log(flags[FlagNames.BattlefieldBrazierLit] + " " + flags[FlagNames.ShadowTerritory1BrazierLit]);
        return (humansKilled >= 5 &&
            shadowsKilled >= 4);
    }
    /// <summary>
    /// Returns whether or not the conditions for the True Human Stage 1 are met
    /// Called by ZoneManager
    /// </summary>
    /// <returns>True if conditions met</returns>
    public bool TrueHumanStage1()
    {
        //Debug.Log(flags[FlagNames.BattlefieldBrazierLit] + " " + flags[FlagNames.ShadowTerritory1BrazierLit]);
        return (flags[FlagNames.BattlefieldBrazierLit] && //Battlefield Brazier must be lit
            humansKilled >= 5 && //Kill all humans without killing any shadows
            shadowsKilled == 0);
    }
    /// <summary>
    /// Flags whether or not a brazier is lit
    /// Should be called whenever a breazier is manipulated
    /// -Connor Menard
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

    public void DialogueEnded(string sequenceName)
    {
        if (sequenceName == "Princess - Saved") 
        {
            GameObject.Find("Princess").GetComponent<Princess>().State = Princess.PrincessStates.Fleeing;
        }
    }
#endregion
}
