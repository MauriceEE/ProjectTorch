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
    private Dictionary<FlagNames, bool> flags;

	void Start () {
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
        flags.Add(FlagNames.DefaultKingOfManDialogue, false);
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
}
