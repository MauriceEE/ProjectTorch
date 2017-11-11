using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class handles buffs/debuffs/etc
/// The StatusEffectManager does the real beef of the work but it uses this class as a relay system
/// </summary>
public class StatusEffect {
    #region Private Fields
    protected float lifetime;
    protected float timeActive;
    protected bool alive;
    protected StatusEffectManager.Buffs buff;
    protected StatusEffectManager.Debuffs debuff;
    protected StatusEffectManager.StatusEffects effect;
    #endregion
    #region Properties
    public float Lifetime { get { return lifetime; } }
    public float TimeActive { get { return timeActive; } }
    public bool Alive { get { return alive; } }
    public StatusEffectManager.Buffs Buff { get { return buff; } }
    public StatusEffectManager.Debuffs Debuff { get { return debuff; } }
    public StatusEffectManager.StatusEffects Effect { get { return effect; } }
    #endregion
    #region Custom Methods
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="_lifetime">How long the status should be applied, in seconds</param>
    /// <param name="_buff">Name of the buff to apply</param>
    /// <param name="_debuff">Name of the debuff to apply</param>
    /// <param name="_effect">Name of the auxiliary effect to apply</param>
    public StatusEffect(float _lifetime, StatusEffectManager.Buffs _buff, StatusEffectManager.Debuffs _debuff, StatusEffectManager.StatusEffects _effect)
    {
        lifetime = _lifetime;
        buff = _buff;
        debuff = _debuff;
        effect = _effect;
        alive = true;
    }
    /// <summary>
    /// Updates time and checks whether or not the effects should be removed
    /// **SHOULD BE CALLED EVERY FRAME BY THE OBJECT THIS IS APPLIED TO!!!**
    /// </summary>
    public void Update()
    {
        timeActive += Time.deltaTime;
        if (timeActive > lifetime)
            alive = false;
    }
    #endregion
}