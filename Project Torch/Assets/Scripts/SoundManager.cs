using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class holds sound data and functions
/// Designed to make sound stuff real easy
/// </summary>
public class SoundManager
{
    #region Enums
    /// <summary>
    /// A list of all sound effect names
    /// **Garret, these are what you should name the effects in Wwise**
    /// </summary>
    public enum SoundEffects
    {
        NONE,//You don't need to put this in Wwise
        //Player
        PlayerSlash,
        PlayerThrust,
        PlayerShine,
        PlayerWalk,
        PlayerDash,
        PlayerHit,
        PlayerDeath,
        PlayerSlashDeflected,
        //Human
        //Normal
        EnemyHumanAttack,
        EnemyHumanWalk,
        EnemyHumanDash,
        EnemyHumanHit,
        EnemyHumanDeath,
        //Brute
        EnemyHumanBruteAttack,
        EnemyHumanBruteWalk,
        EnemyHumanBruteDash,
        EnemyHumanBruteHit,
        EnemyHumanBruteDeath,
        //Spear
        EnemyHumanSpearmanAttack,
        EnemyHumanSpearmanWalk,
        EnemyHumanSpearmanDash,
        EnemyHumanSpearmanHit,
        EnemyHumanSpearmanDeath,
        //Shadow
        //Normal
        EnemyShadowAttack,
        EnemyShadowWalk,
        EnemyShadowDash,
        EnemyShadowHit,
        EnemyShadowDeath,
        //Brute
        EnemyShadowBruteAttack,
        EnemyShadowBruteWalk,
        EnemyShadowBruteDash,
        EnemyShadowBruteHit,
        EnemyShadowBruteDeath,
        //Spear
        EnemyShadowSpearmanAttack,
        EnemyShadowSpearmanWalk,
        EnemyShadowSpearmanDash,
        EnemyShadowSpearmanHit,
        EnemyShadowSpearmanDeath,
        //Glower
        EnemyShadowGlowerAttack,
        EnemyShadowGlowerWalk,
        EnemyShadowGlowerDash,
        EnemyShadowGlowerHit,
        EnemyShadowGlowerDeath,
        EnemyShadowGlowerExplosion,
        //Misc.
        GameEnd,
    }
    /// <summary>
    /// Similar to SoundEffects, this is just an enum for all the music names
    /// **Garret, you'll probably need to rename the music cues to match what's here**
    /// </summary>
    public enum Music
    {
        NONE,//You don't need to implement this in Wwise
        Ambient,
        Combat,
    }
    #endregion
    #region Methods
    /// <summary>
    /// Plays a sound effect
    /// </summary>
    /// <param name="soundEffect">The name of the sound effect</param>
    /// <param name="source">The ID of the game object calling the sound</param>
    public static void PlaySound(SoundEffects soundEffect, GameObject source)
    {
        if (soundEffect != SoundEffects.NONE)
            AkSoundEngine.PostEvent(soundEffect.ToString(), source);
    }
    /// <summary>
    /// Plays music
    /// </summary>
    /// <param name="track">The name of the song to play</param>
    /// <param name="source">The ID of the game object calling the sound</param>
    public static void PlayMusic(Music track, GameObject source)
    {
        if (track != Music.NONE)
            AkSoundEngine.PostEvent(track.ToString(), source);
    }
    /// <summary>
    /// Modifies the volume of a playing sound
    /// </summary>
    /// <param name="soundEffect">Name of the effect</param>
    /// <param name="volume">Volume of the sound effect. Use a scale of zero to one</param>
    public static void SetSoundVolume(SoundEffects soundEffect, float volume)
    {
        if (soundEffect != SoundEffects.NONE)
            AkSoundEngine.SetRTPCValue(soundEffect.ToString(), volume);
    }
    #endregion
}