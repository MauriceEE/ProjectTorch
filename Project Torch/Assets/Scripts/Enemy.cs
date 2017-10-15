using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
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
    public enum EnemyStates
    {
        Idle,
        ApproachingToAttack,
        Attacking,
        SurroundingPlayer,
        ReturningFromAttack,
        ReturningFromEncounter
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
    //Amount of time spent attacking
    protected float attackTime;
    //Scalar used to make hitboxes go left/right
    protected int hitBoxDirectionMove;
    //Reference to player hitbox
    protected GameObject player;
    //Reference to the connected encounter (Note: there doesn't necessarily have to be one)
    protected GameObject encounter;
    // Base color
    protected Color baseColor;
    // reaction variables
    protected bool guarding;
    protected int guardStacks;
    protected int maxGuardStacks = 1;
    protected bool counterattacking;
    protected bool dodging; // probably will need to be an enum state
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
    //If the player stays out of this, the encounter will end
    public float awarenessRange;
    // If enemy is currently attacking
    public bool isAttacking;
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
    //DEBUG//
    public GameObject[] tempHitboxObj;
    [Header("Reaction Chances")]
    public int guardChance;
    public int counterAttackChance;
    public int dodgeChance;
    #endregion

    #region Properties
    public bool Alive { get { return alive; } }
    public bool CanTakeDamage { get { return (damageTimer <= 0f); } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool CanKnockback { get { return !inKnockback; } }
    public Vector2 MoveTarget { get { return moveTarget; } set { moveTarget = value; } }
    public Vector2 ReturnPosition { get { return returnPosition; } set { returnPosition = value; } }
    public Rect HitBoxRect { get { return entity.HitBoxRect; } }
    public GameObject Encounter { get { return encounter; } set { encounter = value; } }
    public float AwarenessRange { get { return awarenessRange; } set { awarenessRange = value; } }
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
        enemyState = EnemyStates.Idle;
        combatState = CombatStates.None;
        player = GameObject.Find("Player");
        // if standard enemy, set chances
        //guardChance = 15;
        //counterAttackChance = 0;
        //dodgeChance = 0;
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

        // color changing for conveyance
        if(hitFlashTimer > 0)
        {
            hitFlashTimer -= Time.deltaTime;
            this.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            // states must be in order from longest active to least active
            if (guarding) baseColor = Color.yellow;
            if (counterattacking) baseColor = new Color((255f/255f), (140f/255f), (30f/255f));
            if (dodging) baseColor = Color.blue;
            if (!dodging && !counterattacking && !guarding) baseColor = Color.white; // default
            this.GetComponent<SpriteRenderer>().color = baseColor;
        }

        switch (enemyState)
        {
            case EnemyStates.Idle:
                //Nothing special for now.........
                isAttacking = false;
                break;
            case EnemyStates.Attacking:
                isAttacking = true;
                attackTime += Time.deltaTime;
                entity.Displacement = Vector2.zero;//Can't move while attacking
                break;
            case EnemyStates.ApproachingToAttack:
                isAttacking = false; // might change this if it proves cheap
                //Update move target
                moveTarget = attackTarget.transform.position;
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
                break;
            case EnemyStates.ReturningFromAttack:
                Debug.Log("State: returningFromAttack");
                //TODO: Stuff the enemy needs to do after making an attack
                isAttacking = false;
                counterattacking = false;
                RequestMoveTarget();//TODO fixed?
                break;
            case EnemyStates.ReturningFromEncounter:
                //moveTarget = returnPosition;
                //Merely move back to the return position and wait
                //TODO: stuff after returning to the return position?
                SeekTarget();
                break;
            case EnemyStates.SurroundingPlayer:
                //Merely follow the enemy manager's orders (it handles updating move target automatically)
                SeekTarget();
                break;
        }

        switch (combatState)
        {
            case CombatStates.Active:
                entity.CanMove = false;
                // --do AABB for box 1--
                AttackRoutine(atHB1, atDamage, atKnockbackTime, atKnockbackSpeed);
                GameObject tempObjBox1 = Instantiate(tempHitboxObj[0] as GameObject, this.transform);
                tempObjBox1.transform.localPosition = new Vector3(atHB1.center.x * hitBoxDirectionMove, atHB1.center.y, 0);
                if (attackTime > (atStartup + atHB2FirstActiveFrame) * Helper.frame)
                {
                    // --do AABB for box 2--
                    AttackRoutine(atHB2, atDamage, atKnockbackTime, atKnockbackSpeed);
                    GameObject tempObjBox2 = Instantiate(tempHitboxObj[1] as GameObject, this.transform);
                    tempObjBox2.transform.localPosition = new Vector3(atHB2.center.x * hitBoxDirectionMove, atHB2.center.y, 0);
                }
                if (attackTime > (atStartup + atHB3FirstActiveFrame) * Helper.frame)
                {
                    // --do AABB for box 3--
                    AttackRoutine(atHB3, atDamage, atKnockbackTime, atKnockbackSpeed);
                    GameObject tempObjBox3 = Instantiate(tempHitboxObj[2] as GameObject, this.transform);
                    tempObjBox3.transform.localPosition = new Vector3(atHB3.center.x * hitBoxDirectionMove, atHB3.center.y, 0);
                }
                if (attackTime > (atStartup + atActive) * Helper.frame)
                    combatState = CombatStates.Recovery;
                break;
            case CombatStates.Recovery:
                entity.CanMove = false;
                if (attackTime > (atStartup + atActive + atRecovery) * Helper.frame)
                {
                    combatState = CombatStates.None;
                    enemyState = EnemyStates.ReturningFromAttack;
                    entity.CanMove = true;
                    //entity.Speed *= 1.5f;
                    attackTime = 0f;
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

        //flag encounter if not yet flagged
        if (!inEncounter)
        {
            //If encounter is assigned, tell it to start an encounter
            if (encounter)
            {
                encounter.GetComponent<Encounter>().StartEncounter();
            }
            else
            {
                //TODO: What if the enemy isn't attached to an encounter?????
                Debug.Log("Encounter object not yet assigned to this enemy!");
            }
        }

        //kill if dead
        if (hp <= 0)
        {
            alive = false;
        }
        // if not dead, try a reaction
        else React();
    }

    private void React()
    {
        // this method will generate a random number between 1 and 100 and check to see if it falls within the ranges of chance for the reactions
        System.Random rand1 = new System.Random();
        int num = (rand1.Next(0, 100)) + 1; // roll to see what number they get
        
        if(num > 0 && num <= guardChance) // if greater than zero (impossible to be false unless an error), and less than guard chance, it is a guard
        {
            // set guarding to true, increment the stacks, halve the movement speed
            guarding = true;
            if(guardStacks < maxGuardStacks) guardStacks++;
            float oldMaxVelocity = maxVelocity;
            float newMaxVelocity = maxVelocity / 2;
            maxVelocity = newMaxVelocity; // CAN'T RESET
            //Debug.Log("Guard Reaction");
        }
        // create and check next threshold, defined as the numbers between the previous reaction chance(s) and the sum of the next reaction chance added to the previous one(s)
        else if(num > guardChance && num <= (counterAttackChance + guardChance))
        {
            // Ask Encounter Manager if it can attack
            if (GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().CanEnemiesAttack())
            {
                counterattacking = true;
                MoveToAttack(player);
                //Debug.Log("CounterAttack Reaction");
            }
            else return;

        }
        // create and check next threshold
        else if(num > (guardChance + counterAttackChance) && num <= (dodgeChance + counterAttackChance + guardChance))
        {
            dodging = true;
            //Debug.Log("Dodge Reaction");
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

    /// <summary>
    /// Unflags inEncounter and tells the enemy to return to its starting position
    /// </summary>
    public void EndEncounter()
    {
        //No longer in an encounter
        inEncounter = false;
        //Set move target to the return position
        this.moveTarget = returnPosition;
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
    /// Returns true if the enemy hit the player
    /// </summary>
    /// <param name="hitbox">Hitbox to check with</param>
    /// <returns>Whether or not the player was hit</returns>
    protected bool CheckCollisionsWithPlayer(Rect hitbox)
    {
        Rect newHB = hitbox;
        if (hitBoxDirectionMove < 0)
            newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
        return (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), player.GetComponent<PlayerCombat>().HitBoxRect));
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
    #endregion

    /// <summary>
    /// Asks the enemy manager for a spot to move to surrounding the player
    /// NOTE: Calling this will also set this enemy to be in the grid surrounding the player
    /// </summary>
    protected void RequestMoveTarget()
    {
        moveTarget = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().SendNewMoveTarget(this);
    }
}
#region Classes
/// <summary>
/// This little bit here draws lines and arcs visually showing the visibility range
/// </summary>
[CustomEditor(typeof(Enemy))]
public class DrawWireArcEnemy : Editor
{
    void OnSceneGUI()
    {
        Handles.color = Color.red;
        Enemy enemy = (Enemy)target;
        Handles.DrawWireArc(enemy.transform.position, new Vector3(0, 0, 1), new Vector3(0, 1, 0), 360f, enemy.awarenessRange);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(0, enemy.awarenessRange, 0), 12f);
        Handles.DrawDottedLine(enemy.transform.position, enemy.transform.position + new Vector3(enemy.awarenessRange, 0, 0), 12f);
    }
}
#endregion