using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base level entity class, should be placed on all moving objects
/// Stuff to know:
///     Properties:
///         Vector2 Speed - max speed of the entity, multiply by a direction vector to get displacement
///         Vector2 Displacement - the amount of Unity units the entity will move when Move() is called
///         bool CanMove - whether or not the entity can move; NOT tested in Move()
///         bool FacingRight - whether or not the entity is facing right, for attacking and sprites
///     Methods:
///         void Move() - translates transform by Displacement and then makes sure it's still in bounds of the level
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour {
    #region Private Fields
    //Movement speed of the entity
    protected Vector2 speed;
    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement = Vector2.zero;
    //Whether or not the entity can move
    protected bool canMove = true;
    //Whether or not the entity is facing right
    protected bool right = true;
    //Reference to the hitbox
    protected BoxCollider2D hitBox;
    //Scale to apply to speed, use with slows and whatnot
    protected float speedModifier;
    //List of status effects
    protected List<StatusEffect> statusEffects;
    //For applying buffs/debuffs
    protected StatusEffectManager statusEffectMan;
    #endregion

    #region Public Fields
    //ID which will be cross-referenced in other managers to determine dialogue
    public TextManager.InteractiveNPCNames npcID;
    #endregion

    #region Properties
    public Vector2 Speed { get { return speed; } set { speed = value; } }
    public Vector2 Displacement { get { return displacement; } set { displacement = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool FacingRight { get { return right; } set { right = value; } }
    public float SpeedModifier { get { return speedModifier; } set { speedModifier = value; } }
    public List<StatusEffect> StatusEffects { get { return statusEffects; } }
    public Rect HitBoxRect { get { return new Rect(new Vector2(hitBox.bounds.center.x, hitBox.bounds.center.y), new Vector2(hitBox.bounds.extents.x, hitBox.bounds.extents.y)); } }
    #endregion

    #region Unity Methods
    void Awake()
    {
        statusEffectMan = GameObject.Find("StatusEffectManagerGO").GetComponent<StatusEffectManager>();
        statusEffects = new List<StatusEffect>(); 
        hitBox = this.GetComponent<BoxCollider2D>();
        speedModifier = 1f;//Always start with default speed
    }
    #endregion

    #region Custom Methods
    public void Move()
    {
        //Attempt to move
        this.transform.position += Helper.Vec2ToVec3(displacement * speedModifier);
        //Footstep sound
        //AkSoundEngine.SetRTPCValue("name", displacement.magnitude);
        //Reset speed modifier for next frame (it must be recalculated)
        speedModifier = 1f;
        //Check to see if we've changed direction
        if (displacement.x != 0) 
        {
            //Update right
            right = (displacement.x > 0);
            //Update sprite if flipping if necessary
            this.GetComponent<SpriteRenderer>().flipX = right;
        }
    }

    /// <summary>
    /// Updates buffs/debuffs on this entity
    /// Should be called every frame
    /// </summary>
    public void UpdateStatusEffects()
    {
        for (int i = 0; i < statusEffects.Count; ++i)
        {
            //Apply the effect
            statusEffectMan.ApplyStatusEffects(statusEffects[i], this.gameObject);
            //Update its time
            statusEffects[i].Update();
            //Check to remove it
            if (!statusEffects[i].Alive)
                statusEffects.RemoveAt(i--);
        }
    }

    /// <summary>
    /// Gives this entity a new status effect
    /// </summary>
    /// <param name="se">StatusEffect to be applied</param>
    public void ApplyNewStatusEffect(StatusEffect se)
    {
        statusEffects.Add(se);
    }

    /// <summary>
    /// These three methods return true if this entity contains the specified status effect
    /// </summary>
    /// <param name="buffName">Name of the buff</param>
    /// <returns>True if contains</returns>
    public bool ContainsStatusEffect(StatusEffectManager.Buffs buffName)
    {
        for (int i = 0; i < statusEffects.Count; ++i)
        {
            if (statusEffects[i].Buff == buffName)
                return true;
        }
        return false;
    }
    public bool ContainsStatusEffect(StatusEffectManager.Debuffs debuffName)
    {
        for (int i = 0; i < statusEffects.Count; ++i)
        {
            if (statusEffects[i].Debuff == debuffName)
                return true;
        }
        return false;
    }
    public bool ContainsStatusEffect(StatusEffectManager.StatusEffects effectName)
    {
        for (int i = 0; i < statusEffects.Count; ++i)
        {
            if (statusEffects[i].Effect == effectName)
                return true;
        }
        return false;
    }
    #endregion
}
