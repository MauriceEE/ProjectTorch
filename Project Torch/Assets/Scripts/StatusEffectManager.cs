using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    #region Enums
    public enum Buffs
    {
        NONE,
    }
    public enum Debuffs
    {
        NONE,
        GlowerSlow,
    }
    public enum StatusEffects
    {
        NONE,
    }
    #endregion
    #region Public Fields
    [Range(0f, 1f)]
    public float glowerSpeedModifierStart;
    [Range(0f, 1f)]
    public float glowerSpeedModifierEnd;
    #endregion
    #region Unity Defautls
    void Start()
    {

    }

    void Update()
    {

    }
    #endregion
    #region Custom Methods
    public void ApplyStatusEffects(StatusEffect effectData, GameObject entity)
    {
        //Check for buffs
        switch (effectData.Buff)
        {
            case Buffs.NONE:
                //Don't do anything
                break;
        }
        //Check for debuffs
        switch (effectData.Debuff)
        {
            case Debuffs.NONE:
                //Don't do anything
                break;
            case Debuffs.GlowerSlow:
                GlowerSlow(effectData, entity);
                break;
        }
        //Check for status effects
        switch (effectData.Effect)
        {
            case StatusEffects.NONE:
                //Don't do anything
                break;
        }
    }
    #endregion
    #region Buffs
    // N/A
    #endregion
    #region Debuffs
    /// <summary>
    /// The Glower's AOE slow
    /// Currently works in this way:
    /// First slows an entity by glowerSpeedModifierStart, 
    ///     eventually decreasing the slow potency down to glowerSpeedModifierEnd, 
    ///     interpolating through the lifetime
    /// </summary>
    /// <param name="effectData">StatusEffect object</param>
    /// <param name="entity">Entity to maniuplate</param>
    protected void GlowerSlow(StatusEffect effectData, GameObject entity)
    {
        entity.GetComponent<Entity>().SpeedModifier *= Mathf.Lerp(glowerSpeedModifierStart, glowerSpeedModifierEnd, effectData.TimeActive / effectData.Lifetime);
    }
    #endregion
    #region Status Effects
    // N/A
    #endregion
}