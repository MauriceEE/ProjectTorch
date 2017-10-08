using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic enemy class
/// Right now it just stands there, takes damage, and eventually dies
/// TODO: basic attack functions
/// 
/// Stuff to know:
///     Properties:
///         bool Alive - whether or not the enemy manager should destroy it
///         bool CanTakeDamage - whether or not the enemy 
/// </summary>
public class Enemy : MonoBehaviour {
    #region Enums
    public enum EnemyState
    {
        Idle,
        ApproachingToAttack,
        Attacking,
        Returning
    }
    protected enum CombatStates
    {
        None,
        Startup,
        Active,
        Recovery,
    }
    #endregion
    #region Private Fields
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
    //Position for the enemy to return to if the player escapes/dies
    protected Vector2 returnPosition;
    //Whether or not this enemy is in a combat encounter
    protected bool inEncounter = false;
    //Current state of the enemy
    protected EnemyState enemyState;
    //Current attack state
    protected CombatStates combatState;
    //Amount of time spent attacking
    protected float attackTime;
    //Scalar used to make hitboxes go left/right
    protected int hitBoxDirectionMove;
    //Reference to player hitbox
    protected GameObject player;
    ///List<Rect> hitboxesCollidedWith  //Resume from here... make a better hitbox delay function
    #endregion
    #region Public Fields
    //Health points
    public float hp;
    //Used to prevent from sliding too far after taking damage
    [Range(0.50f, 1.00f)]
    public float knockbackFriction;
    //Max movement speed
    public float maxVelocity;
    //How far away the enemy will be when it slows down
    public float arrivalRadius;
    //When the enemy will start attacking
    public float attackRange;
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
    #endregion
    #region Properties
    public bool Alive { get { return alive; } }
    public bool CanTakeDamage { get { return (damageTimer <= 0f); } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanKnockback { get { return !inKnockback; } }
    public Vector2 MoveTarget { get { return moveTarget; } set { moveTarget = value; } }
    public Vector2 ReturnPosition { get { return returnPosition; } set { returnPosition = value; } }
    public Rect HitBoxRect { get { return entity.HitBoxRect; } }
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

    void Start () {
        alive = true;
        hitFlashTimer = 0f;
        entity = this.GetComponent<Entity>();
        enemyState = EnemyState.Idle;
        combatState = CombatStates.None;
        player = GameObject.Find("Player");
	}
	
	void Update () {
        //Temp scalar which will affect hitboxes on left/right side
        if (entity.FacingRight)
            hitBoxDirectionMove = 1;
        else
            hitBoxDirectionMove = -1;

        //Damage time limiter
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;

        //Decelerate knockback if being knocked back
        if (inKnockback)
            UpdateKnockback();

        switch (enemyState)
        {
            case EnemyState.Idle:
                //Nothing special for now.........
                break;
            case EnemyState.Attacking:
                //Should this even be here? Might not be necessary
                break;
            case EnemyState.ApproachingToAttack:
                //Move towards player
                SeekTarget();
                //See if you're within range
                if ((this.transform.position - Helper.Vec2ToVec3(moveTarget)).sqrMagnitude <= Mathf.Pow(attackRange, 2)) 
                {

                }
                break;
            case EnemyState.Returning:
                //TODO: Stuff the enemy needs to do after making an attack
                moveTarget = this.transform.position;//temp
                break;
        }

        switch (combatState)
        {
            case CombatStates.Active:
                entity.CanMove = false;
                break;
            case CombatStates.Recovery:
                entity.CanMove = false;
                break;
            case CombatStates.Startup:
                entity.CanMove = false;
                attackTime += Time.deltaTime;
                if (attackTime > atStartup * Helper.frame)
                    combatState = CombatStates.Active;
                break;
            case CombatStates.None:
                if (!inKnockback)
                    entity.CanMove = true;
                break;
        }

        //Move (even if displacement is zero)
        entity.Move();
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Deals HP damage to this enemy
    /// </summary>
    /// <param name="damage">Amout of HP damage the enemy will take</param>
    public void TakeDamage(float damage)
    {
        hp -= damage;
        hitFlashTimer = 0.6f;
        damageTimer = 0.2f;
        //kill if dead
        if (hp < 0)
            alive = false;
        //flag encounter if not yet flagged
        if (!inEncounter)
        {
            GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().StartEncounter(this.transform.position);
            inEncounter = true;
            //StartEncounter();//Actually let's let the enemy manager call this for consistency's sake
        }
    }

    /// <summary>
    /// Call this to make the enemy take knockback
    /// </summary>
    /// <param name="time">Duration IN SECONDS of the knockback</param>
    /// <param name="speed">Initial velocity of the knockback</param>
    public void RecieveKnockback(float time, Vector2 speed)
    {
        canMove = false;
        knockbackTime = time;
        entity.Displacement = speed;
        inKnockback = true;
    }

    /// <summary>
    /// Should be called every frame in knockback
    /// Basically just decelerates the knockback and checks to see if the knockback is over
    /// </summary>
    protected void UpdateKnockback()
    {
        entity.Displacement *= knockbackFriction;
        knockbackTime -= Time.deltaTime;
        if (knockbackTime <= 0)
        {
            canMove = true;
            entity.Displacement = Vector2.zero;
            inKnockback = false;
        }
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
    public void StartEncounter()
    {
        //Remember position so you can go back later
        this.returnPosition = this.transform.position;
        //Flag as in an encounter
        inEncounter = true;
    }

    public void MoveToAttack(Vector3 targetLoc)
    {
        //Assign player as target
        moveTarget = Helper.Vec3ToVec2(targetLoc);
    }

    protected bool CheckCollisions(Rect hitbox)
    {
        Rect newHB = hitbox;
        if (hitBoxDirectionMove < 0)
            newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
        return (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), player.GetComponent<PlayerCombat>().HitBoxRect));
    }

    protected void AttackRoutine(Rect hitbox, float damage, float knockbackTime, float knockbackSpeed)
    {
        if (CheckCollisions(hitbox))
        {
            Debug.Log("PLAYER HIT");
        }
    }
    #endregion
}