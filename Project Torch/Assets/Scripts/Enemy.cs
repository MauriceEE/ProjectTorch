using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
/// <summary>
/// Enemy class. Does a lot of stuff
/// Movement is primarily managed in the enemy manager but the nitty-gritty is executed here
/// UPDATE: As of 10/27, this is now a base abstract class and each enemy game object will need a subclass script
/// </summary>
public abstract class Enemy : MonoBehaviour {
    #region Enums
    public enum EnemyStates
    {
        Idle,
        ApproachingToAttack,
        Attacking,
        Dodging,
        SurroundingPlayer,
        ReturningFromAttack,
        ReturningFromEncounter,
        Knockback,
        Stunned
    }
    public enum CombatStates
    {
        None,
        Startup,
        Active,
        Recovery,
        Stunned
        //Reacting
    }
    protected enum ReactionStates
    {
        None,
        Guard,
        Counter,
        Dodge
    }
    public enum EnemyFaction
    {
        Shadow,
        Human
    }
    #endregion

    #region Private Fields
    //Max HP of the enemy, merely set to current HP at start of game
    protected float maxHP;
    //When set to false the enemy manager will destroy&remove the object
    protected bool alive;
    //For the hit response visual... still a temporary solution???
    protected float hitFlashTimer;
    //Prevents taking damage from multiple sources in the same (very small) time frame
    protected float damageTimer = 0f;
    //Reference to the entity class
    protected Entity entity;
    //Whether or not the enemy can move
    protected bool canMove = true;
    //Whether or not the enemy is being knocked back
    protected bool inKnockback = false;
    //How much time is left on the knockback
    protected float knockbackTime;
    //Target to move towards
    protected Vector2 moveTarget;
    //Target to attack
    protected GameObject attackTarget;
    //Position for the enemy to return to if the player escapes/dies
    protected Vector2 returnPosition;
    //Whether or not this enemy is in a combat encounter
    protected bool inEncounter = false;
    //Current state of the enemy
    protected EnemyStates enemyState;
    //Current attack state
    protected CombatStates combatState;
    //Current reaction state
    protected ReactionStates reactionState;
    //Amount of time spent attacking
    protected float attackTime;
    //Scalar used to make hitboxes go left/right
    protected int hitBoxDirectionMove;
    //Reference to player hitbox
    protected GameObject player;
    //Reference to the connected encounter
    protected GameObject encounter;
    //Enemymanager reference
    protected EnemyManager enemyMan;
    //Time left for the enemy to dash
    protected float dashTime = 0f;
    //Whether the enemy can take damage
    protected bool invincible = false;
    //Whether or not the enemy's guard is broken
    protected bool guardBroken = false;
    //Duration remaining of a stun
    protected float stunTime;
    //Multiplier applied to knockback
    protected float knockbackModifier;
    //Whether or not this enemy is allied with the player, will start as true
    protected bool alliedWithPlayer;
    //Used for resetting
    protected Vector3 startingPosition;
    //Whether or not this enemy is being attacked by another
    //protected bool targetedByEnemy;
    // Base color
    protected Color baseColor;
    // reaction variables
    protected bool guarding;
    protected int guardStacks;
    protected int maxGuardStacks = 1;
    protected bool counterattacking;
    protected bool dodging; // probably will need to be an enum state
    protected float elapsedApproachTime;
    ///List<Rect> hitboxesCollidedWith  //I'll make a better hitbox function if I have time someday
                                        // I gotchu, fam
    protected bool hitPlayer;
    // hits taken recently
    protected int hitsTakenRecently;
    protected float increasedReactionWindowTimer;
    protected float initialIRWT;
    protected int irwtChance;
    protected int irwGuardChance;
    protected int irwDodgeChance;
    protected int irwCounterAttackChance;
    protected string irwType;
    protected float ogAtStartup;
    // bool for if the attack sound has played already
    protected bool attackAudioPlayed;
    #endregion

    #region Public Fields
    //Health points
    public float hp;
    //Faction of this enemy
    public EnemyFaction faction;
    //Zone the enemy is in
    public ZoneManager.ZoneNames zone;
    //Used to prevent from sliding too far after taking damage
    [Range(0.50f, 1.00f)]
    public float knockbackFriction;
    //Duration of time to be stunned after a guard break
    public float guardBreakStunTime;
    //Max movement speed
    public float maxVelocity;
    //The original max velocity
    protected float ogMaxVelocity;
    //How far away the enemy will be when it slows down
    public float arrivalRadius;
    //When the enemy will start attacking
    public float attackRange;
    //The original attackRange
    protected float ogAttackRange;
    //If the player stays out of this, the encounter will end
    public float awarenessRange;
    // If enemy is currently attacking
    protected bool isAttacking;
    [Header("Attack data")]
    public float atDamage;
    public float atStartup;
    public float atActive;
    public float atRecovery;
    public float atKnockbackTime;
    public float atKnockbackSpeed;
    public bool atEditorHitboxes = true;//DEBUG//
    public Rect atHB1;
    public float atHB2FirstActiveFrame;
    public Rect atHB2;
    public float atHB3FirstActiveFrame;
    public Rect atHB3;
    [Header("Dash data")]
    public float dashIFrames = 6;
    public float dashFrames = 10;
    public float dashSpeed;
    //Makes the dash more smooth
    [Range(0.50f, 1.00f)]
    public float dashFriction;
    [Header("Reaction Chances")]
    public int guardChance;
    public int counterAttackChance;
    public int dodgeChance;
    //DEBUG//
    [Header("//DEBUG//")]
    public GameObject[] tempHitboxObj;
    #endregion

    #region Properties
    public bool Alive { get { return alive; } set { alive = value; } }
    public float MaxHP { get { return maxHP; } }
    public bool IsAttacking { get { return isAttacking; } }
    public bool CanTakeDamage { get { return (damageTimer <= 0f && !invincible); } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanKnockback { get { return !inKnockback; } }
    public Vector2 MoveTarget { get { return moveTarget; } set { moveTarget = value; } }
    public Vector2 ReturnPosition { get { return returnPosition; } set { returnPosition = value; } }
    public Rect HitBoxRect { get { return entity.HitBoxRect; } }
    public GameObject Encounter { get { return encounter; } set { encounter = value; } }
    public float AwarenessRange { get { return awarenessRange; } set { awarenessRange = value; } }
    public bool GuardBroken { get { return guardBroken; } set { guardBroken = value; } }
    public float KnockbackModifier { get { return knockbackModifier; } }
    public bool AlliedWithPlayer { get { return alliedWithPlayer; } set { alliedWithPlayer = value; } }
    public GameObject AttackTarget { get { return attackTarget; } set { attackTarget = value; } }
    public EnemyStates EnemyState { get { return enemyState; } }
    public CombatStates CombatState { get { return combatState; } }
    public Vector3 StartingPosition { get { return startingPosition; } }
    #endregion

    #region Unity Methods
    private void OnDrawGizmos()
    {
        if(atEditorHitboxes)
        {
            Helper.DebugDrawRect(this.transform.position, atHB1);
            Helper.DebugDrawRect(this.transform.position, atHB2);
            Helper.DebugDrawRect(this.transform.position, atHB3);
        }
    }

    protected virtual void Awake () {
        alive = true;
        hitFlashTimer = 0f;
        entity = this.GetComponent<Entity>();
        enemyState = EnemyStates.Idle;
        combatState = CombatStates.None;
        reactionState = ReactionStates.None;
        player = GameObject.Find("Player");
        knockbackModifier = 1f;
        ogMaxVelocity = maxVelocity;
        ogAttackRange = attackRange;
        elapsedApproachTime = 0;
        alliedWithPlayer = true;
        // if standard enemy, set chances
        //guardChance = 15;
        //counterAttackChance = 0;
        //dodgeChance = 0;
        hitPlayer = false;
        maxHP = hp;
        startingPosition = this.transform.position;
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        attackAudioPlayed = false;
        hitsTakenRecently = 0;
        increasedReactionWindowTimer = 1.5f;
        initialIRWT = increasedReactionWindowTimer;
        irwtChance = 0;
        irwGuardChance = 0;
        irwDodgeChance = 0;
        irwCounterAttackChance = 0;
        irwType = "dodge";
        ogAtStartup = atStartup;
    }
	
	protected virtual void Update () {
        //Temp scalar which will affect hitboxes on left/right side
        if (entity.FacingRight)
            hitBoxDirectionMove = 1;
        else
            hitBoxDirectionMove = -1;

        //Damage time limiter
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;

        // check if the enemy has taken hits recently
        if(hitsTakenRecently > 0)
        {
            // decrease timer determining window of time
            increasedReactionWindowTimer -= Time.deltaTime;

            // if window has closed, reset values
            if (increasedReactionWindowTimer <= 0)
            {
                hitsTakenRecently = 0;
                increasedReactionWindowTimer = initialIRWT;
                //Debug.Log("Reset IRWT");
            }
        }

        // color changing for conveyance
        UpdateColor();

        //Apply status effects
        entity.UpdateStatusEffects();

        //Check stun recovery
        if (guardBroken)
        {
            //Reset knockback modifier
            knockbackModifier = 1f;
            //Reset guard broken
            guardBroken = false;
            //Reset guarding
            guarding = false;
            //Reset speed modifier
            entity.SpeedModifier = 1f;
        }

        //Manage states
        UpdateEnemyState();
        UpdateCombatState();
        stunTime -= Time.deltaTime;
        if (stunTime < 0f)
            stunTime = 0f;

        //Update speed in entity
        entity.Speed = Helper.Map(entity.Displacement.sqrMagnitude, 0f, maxVelocity * maxVelocity, 0f, 1f);

        //Move (even if displacement is zero)
        entity.Move();
    }
    #endregion

    #region Update Methods (run every frame)
    protected virtual void UpdateColor()
    {
        if (hitFlashTimer > 0)
        {
            hitFlashTimer -= Time.deltaTime;
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            // states must be in order from longest active to least active
            if (guarding) baseColor = Color.yellow;
            if (counterattacking) baseColor = new Color((255f / 255f), (140f / 255f), (30f / 255f));
            if (dodging) baseColor = Color.blue;
            if (!dodging && !counterattacking && !guarding) baseColor = Color.white; // default
            if (combatState == CombatStates.Startup) baseColor = new Color((255f / 255f), (0f / 255f), (255f / 255f));
            this.GetComponent<SpriteRenderer>().color = baseColor;
        }
    }

    /// <summary>
    /// Handles combatState, called every frame
    /// </summary>
    protected virtual void UpdateCombatState()
    {
        //Second big switch handling combat
        switch (combatState)
        {
            case CombatStates.Active:
                entity.CanMove = false;
                // --do AABB for box 1--
                if (!alliedWithPlayer)
                    AttackRoutine(atHB1, atDamage, atKnockbackTime, atKnockbackSpeed);
                else
                    CheckCollisionsWithEnemies(atHB1);
                GameObject tempObjBox1 = Instantiate(tempHitboxObj[0] as GameObject, this.transform);
                tempObjBox1.transform.localPosition = new Vector3(atHB1.center.x * hitBoxDirectionMove, atHB1.center.y, 0);
                if (attackTime > (atStartup + atHB2FirstActiveFrame) * Helper.frame)
                {
                    // --do AABB for box 2--
                    if (!alliedWithPlayer)
                        AttackRoutine(atHB2, atDamage, atKnockbackTime, atKnockbackSpeed);
                    else
                        CheckCollisionsWithEnemies(atHB2);
                    GameObject tempObjBox2 = Instantiate(tempHitboxObj[1] as GameObject, this.transform);
                    tempObjBox2.transform.localPosition = new Vector3(atHB2.center.x * hitBoxDirectionMove, atHB2.center.y, 0);
                }
                if (attackTime > (atStartup + atHB3FirstActiveFrame) * Helper.frame)
                {
                    // --do AABB for box 3--
                    if (!alliedWithPlayer)
                        AttackRoutine(atHB3, atDamage, atKnockbackTime, atKnockbackSpeed);
                    else
                        CheckCollisionsWithEnemies(atHB3);
                    GameObject tempObjBox3 = Instantiate(tempHitboxObj[2] as GameObject, this.transform);
                    tempObjBox3.transform.localPosition = new Vector3(atHB3.center.x * hitBoxDirectionMove, atHB3.center.y, 0);
                }
                if (attackTime > (atStartup + atActive) * Helper.frame)
                    combatState = CombatStates.Recovery;
                break;
            case CombatStates.Recovery:
                maxVelocity = ogMaxVelocity;
                entity.CanMove = false;
                if (attackTime > (atStartup + atActive + atRecovery) * Helper.frame)
                {
                    combatState = CombatStates.None;
                    enemyState = EnemyStates.ReturningFromAttack;
                    entity.CanMove = true;
                    //entity.Speed *= 1.5f;
                    attackTime = 0f;
                    hitPlayer = false;
                }
                break;
            case CombatStates.Startup:
                entity.CanMove = false;
                if (attackTime > atStartup * Helper.frame)
                    combatState = CombatStates.Active;
                break;
            case CombatStates.None:
                //if (!inKnockback)
                //entity.CanMove = true;
                break;
            case CombatStates.Stunned:
                if (stunTime < 0f)
                    ResetCombatStates();
                break;
        }
    }

    /// <summary>
    /// Handles enemyState, called every frame
    /// </summary>
    protected virtual void UpdateEnemyState()
    {
        // stop being invincible, ya butt
        if(enemyState != EnemyStates.Dodging)
        {
            dodging = false;
            invincible = false;
        }

        //Big switch over the current state
        switch (enemyState)
        {
            case EnemyStates.Idle:
                //Nothing special for now.........
                isAttacking = false;
                maxVelocity = ogMaxVelocity;
                //Don't do nothing if you're in an encounter
                if (inEncounter)
                {
                    if (!alliedWithPlayer)
                        RequestMoveTarget();
                    else
                        enemyMan.SendAllyAttackOrder(this);
                }
                else
                    this.moveTarget = this.transform.position;
                break;
            case EnemyStates.Attacking:
                elapsedApproachTime = 0f;
                attackRange = ogAttackRange;
                isAttacking = true;
                attackTime += Time.deltaTime;
                entity.Displacement = Vector2.zero;//Can't move while attacking
                break;
            case EnemyStates.ApproachingToAttack:
                isAttacking = false; // might change this if it proves cheap
                // increase movement speed
                if (maxVelocity <= (3 * ogMaxVelocity))
                    maxVelocity += (Time.deltaTime / 15);
                elapsedApproachTime += Time.deltaTime;
                // adjust attack range to increase likelihood of a chasing attack landing
                if (elapsedApproachTime >= 1)
                {
                    attackRange = .8f;
                    //atStartup = ogAtStartup - 10;
                }
                // if elapsed time is greater than 3 seconds, just cancel their attack and return them to their old location
                if (elapsedApproachTime > 3)
                {
                    CancelOrHitStun(false);
                    enemyState = EnemyStates.ReturningFromAttack;
                    elapsedApproachTime = 0f;

                    // alternative: enemies attempt to attack after approaching for 3 seconds
                    /*
                    //Change states accordingly
                    combatState = CombatStates.Startup;
                    enemyState = EnemyStates.Attacking;
                    attackTime = 0f;
                    Debug.Log("Chase slash attempt");
                    */
                }
                //Update move target
                if (attackTarget)
                    moveTarget = attackTarget.transform.position;
                else
                    Debug.Log("No attack target!");
                //Move towards target
                SeekTarget();
                //See if you're within range
                if ((this.transform.position - Helper.Vec2ToVec3(moveTarget)).sqrMagnitude <= Mathf.Pow(attackRange, 2))
                {
                    //Change states accordingly
                    combatState = CombatStates.Startup;
                    enemyState = EnemyStates.Attacking;
                    attackTime = 0f;
                }
                attackRange = ogAttackRange;
                //atStartup = ogAtStartup;
                break;
            case EnemyStates.ReturningFromAttack:
                isAttacking = false;
                counterattacking = false;
                RequestMoveTarget();
                maxVelocity = ogMaxVelocity;
                break;
            case EnemyStates.ReturningFromEncounter:
                //moveTarget = returnPosition;
                //Merely move back to the return position and wait
                //TODO: stuff after returning to the return position?
                SeekTarget();
                if ((Helper.Vec3ToVec2(this.transform.position) - moveTarget).sqrMagnitude <= arrivalRadius)
                {
                    enemyState = EnemyStates.Idle;
                    //Prevent the enemy from continuously moving 
                    this.moveTarget = this.transform.position;
                }
                break;
            case EnemyStates.SurroundingPlayer:
                //Merely follow the enemy manager's orders (it handles updating move target automatically)
                // IDEALLY: IF MOVING TOWARDS THE PLAYER, KEEP MAX MOVEMENT. IF NOT, REDUCE IT TO 1/3rd
                maxVelocity = ogMaxVelocity / 2;
                SeekTarget();
                break;
            case EnemyStates.Dodging:
                //Continue the dodge action
                SeekTarget();
                dashTime -= Time.deltaTime;
                //IF the dodge is over...
                if (dashTime <= 0f)
                {
                    //Reset flag
                    dodging = false;
                    //Return from attack (i.e. let enemy manager figure out what to do next)
                    enemyState = EnemyStates.ReturningFromAttack;
                    //Return speed to normal
                    entity.SpeedModifier = 1f;
                    //Reset invinicibility
                    invincible = false;
                }
                //If you're still dodging...
                else
                {
                    //Check to see if you're still invincible
                    invincible = (dashTime > (dashFrames - dashIFrames) * Helper.frame);
                    //Scale down displacement with dampening
                    //entity.Displacement *= dashFriction;//What the player version uses
                    entity.SpeedModifier *= dashFriction;
                }
                break;
            case EnemyStates.Knockback:
                UpdateKnockback();
                break;
            case EnemyStates.Stunned:
                if (stunTime < 0f)
                    ResetCombatStates();
                break;
            default:
                Debug.Log("ENEMY DOES NOT HAVE A STATE!!!");
                break;
        }
    }

    /// <summary>
    /// Should be called every frame in knockback
    /// Basically just decelerates the knockback and checks to see if the knockback is over
    /// </summary>
    protected virtual void UpdateKnockback()
    {
        entity.Displacement *= knockbackFriction;
        knockbackTime -= Time.deltaTime;
        if (knockbackTime <= 0)
        {
            canMove = true;
            entity.Displacement = Vector2.zero;
            inKnockback = false;
            enemyState = EnemyStates.Idle;
        }
        else
            enemyState = EnemyStates.Knockback;
    }
    #endregion
    #region Combat Methods
    /// <summary>
    /// Deals HP damage to this enemy
    /// </summary>
    /// <param name="damage">Amout of HP damage the enemy will take</param>
    public virtual void TakeDamage(float damage, PlayerCombat.Attacks attackType)
    {
        //Only actually take damage if your guard is down
        if (guardStacks == 0)
        {
            hp -= damage;
            //Flag as dead if out of HP
            if (hp <= 0)
            {
                alive = false;
                return;//Get outta here to avoid wasting time on the other code
            }
            hitsTakenRecently++; // increment number of recent hits taken
            increasedReactionWindowTimer = initialIRWT; // reset IRWT
            CancelOrHitStun(true);
            hitFlashTimer = 0.6f;
            damageTimer = 0.2f;
        }

        //Get out of stun if your guard was broken and you're taking damage
        //        if (guardBroken)
        if (enemyState == EnemyStates.Stunned || enemyState == EnemyStates.Knockback) 
        {
            //Reset stun
            stunTime = 0f;
            ResetCombatStates();
            Debug.Log("attack after guard break @ " + Time.fixedTime);
        }

        //Flag encounter if not yet flagged
        if (!inEncounter)
        {
            //If encounter is assigned, tell it to start an encounter
            if (encounter)
            {
                encounter.GetComponent<Encounter>().StartEncounter(this.faction);
            }
            else
            {
                //TODO: What if the enemy isn't attached to an encounter?????
                Debug.Log("Encounter object not yet assigned to this enemy!");
            }
        }
        else
        {
            if (alliedWithPlayer)
            {
                //Add to current aggression if allied with the player
                enemyMan.EncounterAggressionCurrent += damage;
            }
        }

        //If guard up...
        if (guardStacks > 0) 
        {
            //If hit by thrust...
            if (attackType == PlayerCombat.Attacks.Thrust)
                //Remove stack, and if that was the last one...
                if (--guardStacks <= 0)
                {
                    //Break their guard
                    BreakGuard(2f);
                    guarding = false;
                }
            //Debug.Log(guardStacks);
            //TODO: If hit by slash, do some reboud fancy thing
            if (attackType == PlayerCombat.Attacks.Slash)
            {
                Debug.Log("TODO: REBOUND THINGEY");
            }
        }

        // if not dead, try a reaction if not already trying one
        //        if (!guarding && !dodging && !counterattacking && !guardBroken)
        if (!guarding && !dodging && !counterattacking && enemyState != EnemyStates.Stunned && enemyState != EnemyStates.Knockback)
            React();
    }

    /// <summary>
    /// This used to be a part of TakeDamage but I found it was useful to reuse it for Shine counters too
    /// </summary>
    /// <param name="knockbackMultiplier">How much the next hit of knockback will be multiplied by</param>
    public virtual void BreakGuard(float knockbackMultiplier)
    {
        //Flag guard broken state
//        guardBroken = true;
        //Get stunned
        stunTime = guardBreakStunTime;
        //Modify knockback
        knockbackModifier = knockbackMultiplier;
        //Reset states
        ResetCombatStates();
        //Update states to stun
        enemyState = EnemyStates.Stunned;
        combatState = CombatStates.Stunned;
        Debug.Log("Guard broken @ " + Time.fixedTime);
    }

    /// <summary>
    /// Resets some variables to restore the enemy to a neutral state in combat
    /// </summary>
    public void ResetCombatStates()
    {
        invincible = false;
        enemyState = EnemyStates.Idle;
        combatState = CombatStates.None;
        isAttacking = false;
        attackTime = 0f;
        elapsedApproachTime = 0f;
        knockbackModifier = 1f;
        guarding = false;
        dodging = false;
        counterattacking = false;
        entity.SpeedModifier = 1f;
        hitPlayer = false;
    }

    protected virtual void React()
    {
        //Generate random chance between 0% and 100% (represented as 0 to 100)
        float rand = Random.Range(0f, 100f);

        // Set irw chance
        irwtChance = (hitsTakenRecently - 1) * 40;
        irwtChance = Mathf.Clamp(irwtChance, 0, 100);
        //Debug.Log(irwtChance);

        // get irw type and set values
        switch(irwType.ToLower())
        {
            case "dodge":
                irwDodgeChance = irwtChance;
                //if (irwGuardChance > 0) Debug.Log("Chance up");
                break;
            case "counterattack":
                irwCounterAttackChance = irwtChance / 2;
                break;
            case "guard":
                irwGuardChance = irwtChance;
                break;
        }

        //Check to see if we fell within guard's percent chance
        if (rand < guardChance + irwGuardChance)
        {
            //Enter guarding state
            guarding = true;
            //Increment stacks but stay within bounds
            if (++guardStacks > maxGuardStacks)
                guardStacks = maxGuardStacks;
            //Halve speed
            this.entity.SpeedModifier *= 0.5f;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
        }
        else if (rand < guardChance + counterAttackChance + irwCounterAttackChance)
        {
            // Ask Encounter Manager if it can attack
            if (enemyMan.CanEnemiesAttackPlayer())
            {
                //Enter counterattack state
                counterattacking = true;
                //Attack the player
                MoveToAttack(attackTarget);
            }
        }
        else if (rand < guardChance + counterAttackChance + dodgeChance + ((maxHP - hp) / 2) + irwDodgeChance) // increase chance to dodge based on inherent chance and how much health has been lost
        {
            Dodge();
        }
    }

    public void Dodge()
    {
        //Flag as dodging
        dodging = true;
        //Enter dodging state
        enemyState = EnemyStates.Dodging;
        //Set new move target away from player
        moveTarget = this.transform.position + (this.transform.position - player.transform.position) * 2;
        //Set duration of dash
        dashTime = dashFrames * Helper.frame;
        //Modify speed
        entity.SpeedModifier *= dashSpeed;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
        //Remove from occupancy grid
        RequestRemoveFromEncounterGrid();
    }

    /// <summary>
    /// Call this to make the enemy take knockback
    /// </summary>
    /// <param name="time">Duration IN SECONDS of the knockback</param>
    /// <param name="speed">Initial velocity of the knockback</param>
    public virtual void RecieveKnockback(float time, Vector2 speed)
    {
        canMove = false;
        knockbackTime = time;
        entity.Displacement = speed * knockbackModifier;
        //reset knockback modifer
        knockbackModifier = 1f;
        inKnockback = true;
        ResetCombatStates();
        enemyState = EnemyStates.Knockback;
    }

    // Cancels any and all frames
    /// <param name="hitstun">Whether or not to apply stun with the cancel</param>
    protected virtual void CancelOrHitStun(bool hitstun)
    {
        // Set combat state to Recovery
        //combatState = CombatStates.Recovery; // causes RunTime error

        // Set attackTime to the max float value to immediately end current state's frames
        attackTime = float.MaxValue;

        // reset dash time
        //dashTime = .00000001f; // incredibly small positive number so the next frame will end the dash

        // if hitstun, set hitstun frames
        if (hitstun) stunTime = 20f * Helper.frame;
        // *Coded by Maurice Edwards
    }


    /// <summary>
    /// Returns true if the enemy hit the player
    /// </summary>
    /// <param name="hitbox">Hitbox to check with</param>
    /// <returns>Whether or not the player was hit</returns>
    protected bool CheckCollisionsWithPlayer(Rect hitbox)
    {
        Rect newHB = hitbox;
        if (hitBoxDirectionMove < 0)
            newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
        // check if the enemy has hit the player before the hitPlayer variable was returned to false.
        // IF THIS METHOD IS EVER USED TO CHECK COLLISIONS WITH ANYTHING BESIDES THE PLAYER, ENSURE THE CHECK ONLY HAPPENS IF THE TARGET IS THE PLAYER
        bool hit = (hitPlayer == false && (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), player.GetComponent<PlayerCombat>().HitBoxRect)));
        if (hit == true) hitPlayer = true; // set hitPlayer to true so this attack can't hit more than once
        return hit;
    }

    /// <summary>
    /// Checks hitbox collisions with enemies 
    /// ***AND applies damage***
    /// </summary>
    /// <param name="hitbox">Hitbox to check</param>
    /// <param name="enemies">List of enemies to attack</param>
    /// <returns>Whether or not they hit</returns>
    protected bool CheckCollisionsWithEnemies(Rect hitbox)
    {
        //Don't bother with any of this if we hit an enemy this attack
        if(!hitPlayer)
        {
            Rect newHB = hitbox;
            Enemy e;
            if (hitBoxDirectionMove < 0)
                newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
            //Loop through all enemies
            for (int i = 0; i < enemyMan.encounterEnemies.Count; ++i)
            {
                e = enemyMan.encounterEnemies[i].GetComponent<Enemy>();
                //Only bother with ones that aren't allied with the player
                if (!e.alliedWithPlayer)
                {
                    //See if our hitbox hit them
                    if (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), e.HitBoxRect)) 
                    {
                        hitPlayer = true;
                        e.TakeDamage(atDamage, PlayerCombat.Attacks.Slash);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Handles all the necessary attacking methods
    /// TODO: Check whether or not allied with player
    /// TODO: Damage other enemies of opposing factions
    /// </summary>
    /// <param name="hitbox">Hitbox to check</param>
    /// <param name="damage">Damage of attack</param>
    /// <param name="knockbackTime">Knockback time of attack</param>
    /// <param name="knockbackSpeed">Knockback speed of attack</param>
    protected void AttackRoutine(Rect hitbox, float damage, float knockbackTime, float knockbackSpeed)
    {
        if (CheckCollisionsWithPlayer(hitbox))
        {
            player.GetComponent<PlayerCombat>().TakeDamage(atDamage);
        }
    }
    #endregion
    #region Encounter Methods
    /// <summary>
    /// Asks the enemy manager to remove this enemy from the surrounding occupancy grid
    /// </summary>
    protected void RequestRemoveFromEncounterGrid()
    {
        enemyMan.RemoveEnemyFromOccupancyGrid(this);
    }

    /// <summary>
    /// Seeks out the 'moveTarget' vector location
    /// </summary>
    public void SeekTarget()
    {
        //Get deisred velocity
        Vector2 desiredDisplacement = moveTarget - new Vector2(this.transform.position.x, this.transform.position.y);
        //Apply displacement
        this.entity.Displacement += desiredDisplacement;
        //Limit based on max speed
        this.entity.Displacement = Vector2.ClampMagnitude(this.entity.Displacement, maxVelocity);
        //See if you're close enough to slow down
        float dist = desiredDisplacement.magnitude;
        if (dist <= arrivalRadius)
            this.entity.Displacement *= dist / arrivalRadius;
    }

    /// <summary>
    /// Sets up stuff necessary for encounters
    /// Should be called if in an encounter but not called yet
    /// </summary>
    public virtual void StartEncounter()
    {
        //Remember position so you can go back later
        this.returnPosition = this.transform.position;
        //Flag as in an encounter
        inEncounter = true;
    }

    /// <summary>
    /// Unflags inEncounter and tells the enemy to return to its starting position
    /// </summary>
    public void EndEncounter()
    {
        //No longer in an encounter
        inEncounter = false;
        //Fix abnormal states
        ResetCombatStates();
        //Set move target to the return position
        this.moveTarget = startingPosition;
        //Set state to returning to said position
        this.enemyState = EnemyStates.ReturningFromEncounter;
    }

    /// <summary>
    /// Orders this enemy to engage an attack
    /// </summary>
    /// <param name="targetObj">Target to attack</param>
    public void MoveToAttack(GameObject targetObj)
    {
        //Assign player as target
        attackTarget = targetObj;
        //Change to move to attack state
        enemyState = EnemyStates.ApproachingToAttack;
    }

    /// <summary>
    /// Tells this enemy to circle around the player.
    /// This is done by changing the enemy's current state,
    /// nothing else at the moment
    /// </summary>
    public void CircleAroundPlayer()
    {
        this.enemyState = EnemyStates.SurroundingPlayer;
    }

    /// <summary>
    /// Assigns this enemy to an encounter
    /// Should be called if enemy runs within range of an active encounter,
    ///     or if an encounter begins with this enemy inside its range
    /// </summary>
    /// <param name="enc">Encounter gameobject to be added to</param>
    public void AssignToEncounter(GameObject enc)
    {
        this.encounter = enc;
        this.encounter.GetComponent<Encounter>().TriggerEnemies.Add(this.gameObject);
    }

    /// <summary>
    /// Asks the enemy manager for a spot to move to surrounding the player
    /// NOTE: Calling this will also set this enemy to be in the grid surrounding the player
    /// </summary>
    protected void RequestMoveTarget()
    {
        moveTarget = enemyMan.SendNewMoveTarget(this);
    }
    #endregion
}