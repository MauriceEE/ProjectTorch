﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deals with all manner of player combat functions
/// Hitboxes, frame data, attack states, and more
/// </summary>
public class PlayerCombat : MonoBehaviour {
    #region Enums
    protected enum CombatStates
    {
        None,
        Startup,
        Active,
        Recovery,
    }    
    public enum Attacks
    {
        None,
        Slash,
        Thrust,
        Shine
    }
    #endregion

    #region Public Fields
    public float hp = 100f;
    [Header("Slash data")]
    public float slDamage = 10;
    public float slStartup = 6;
    public float slActive = 10;
    public float slRecovery = 8;
    public float slKnockbackTime;
    public float slKnockbackSpeed;
    public bool slEditorHitboxes = true;//DEBUG//
    public Rect slHB1;
    public float slHB2FirstActiveFrame = 4;
    public Rect slHB2;
    public float slHB3FirstActiveFrame = 7;
    public Rect slHB3;
    [Space(10)]
    [Header("Thrust data")]
    public float thDamage = 15;
    public float thStartup = 12;
    public float thActive = 7;
    public float thRecovery = 10;
    public float thKnockbackTime;
    public float thKnockbackSpeed;
    public bool thEditorHitboxes = true;//DEBUG//
    public Rect thHB1;
    public float thHB2FirstActiveFrame = 3;
    public Rect thHB2;
    public float thHB3FirstActiveFrame = 5;
    public Rect thHB3;[Space(10)]
    [Header("Shine data")]
    public float shStartup = 12;
    public float shActive = 7;
    public float shRecovery = 10;
    public bool shEditorHitboxes = true;//DEBUG//
    public Rect[] shHB;
    [Header("Debug")]
    public GameObject[] tempHitboxObj;
    #endregion

    #region Private Fields
    //Time spent since attack began
    protected float attackTime = 0f;
    //Whether or not the player can attack, can be accessed and modified by other scripts
    private bool canAttack = true;
    //Whether or not the player is holding the torch
    protected bool holdingTorch = true;
    //Reference to the movement class
    protected PlayerMovement movement;
    //Reference to the entity class
    protected Entity entity;
    //Reference to enemy manager
    protected EnemyManager enemyMan;
    //Used to attack left/right
    protected int hitBoxDirectionMove;
    //The player's combat state
    protected CombatStates combatState;
    //The player's current attack state
    protected Attacks currentAttack;
    //A list of hit enemies, used in collision detection
    protected List<Enemy> hitEnemies;
    //Number of consecutive Slashes
    private int consecSlashCount = 0;
    //List of enemies you can hit this frame
    protected List<GameObject> hittableEnemies;
    #endregion

    #region Properties
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    public Rect HitBoxRect { get { return entity.HitBoxRect; } }
    #endregion

    #region Unity Methods
    void Start()
    {
        combatState = CombatStates.None;
        currentAttack = Attacks.None;
        entity = this.GetComponent<Entity>();
        movement = this.GetComponent<PlayerMovement>();
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
    }

    void Update()
    {
        //Temp scalar which will affect hitboxes on left/right side
        if (entity.FacingRight)
            hitBoxDirectionMove = 1;
        else
            hitBoxDirectionMove = -1;

        
        attackTime += Time.deltaTime;

        //Cancel attack
        if (Input.GetKeyDown(KeyCode.Space)) Cancel();

        //Check to see if the player can attack
        if (canAttack)
        {
            // Detect input for any attacks out of neutral stance and any that chain from Slash while it is in Recovery
            if (combatState == CombatStates.None || (currentAttack == Attacks.Slash && combatState == CombatStates.Recovery))
            {
                //See if they input an attack button
                if (consecSlashCount < 2 && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J)))//SQUARE / X
                {
                    // If in Slash's Recovery frames still, cancel it
                    if (combatState == CombatStates.Recovery)
                    {
                        Cancel();

                        // Go through Slash's pertinent recovery code
                        canAttack = true;
                        entity.CanMove = true;
                        entity.Speed *= 1.5f;

                        // increment Slash count
                        consecSlashCount++;
                    }

                    attackTime = 0f;
                    //Start attacking
                    combatState = CombatStates.Startup;
                    currentAttack = Attacks.Slash;
                    if (consecSlashCount == 2) canAttack = false;
                    //entity.CanMove = false;
                    entity.Speed /= 1.5f;
                    movement.CanDash = false;
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.K))//TRIANGLE / Y
                {
                    // Reset consecutive Slash count since this isn't a Slash attack
                    consecSlashCount = 0;
                    // If in Slash's Recovery frames still, cancel it
                    if (combatState == CombatStates.Recovery)
                    {
                        Cancel();
                        // Reset player speed from the slow movement from Slash's start-up
                        entity.Speed *= 1.5f;

                        // increment Slash count
                        consecSlashCount++;
                    }
                    attackTime = 0f;
                    //Start attacking
                    combatState = CombatStates.Startup;
                    currentAttack = Attacks.Thrust;
                    canAttack = false;
                    entity.CanMove = false;
                    movement.CanDash = false;
                }

                // PUT SHINE CODE HERE since it will also chain from Slash
                if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.I))//R1
                {
                    //Reset consecutive slashes
                    consecSlashCount = 0;
                    //Cancel from recovery frames
                    if (combatState == CombatStates.Recovery)
                    {
                        Cancel();
                        //Reset speed from slash's startup
                        entity.Speed *= 1.5f;
                        //increment slash count
                        ++consecSlashCount;
                    }
                    attackTime = 0f;
                    //Start shining
                    combatState = CombatStates.Startup;
                    currentAttack = Attacks.Shine;
                    canAttack = false;
                    entity.CanMove = false;
                    movement.CanDash = false;
                }

            } // end chain attacks from Slash
        }
        //DON'T make this an else or the player won't actually attack until one frame later
        if (combatState == CombatStates.Startup)
        {
            movement.CanDash = false;
            switch (currentAttack)
            {
                //Check to move to active frames
                case Attacks.Slash:
                    if (attackTime > slStartup * Helper.frame)
                        combatState = CombatStates.Active;
                    break;
                case Attacks.Thrust:
                    if (attackTime > thStartup * Helper.frame)
                        combatState = CombatStates.Active;
                    break;
                case Attacks.Shine:
                    if (attackTime > shStartup * Helper.frame)
                        combatState = CombatStates.Active;
                    break;
            }
        }
        if (combatState == CombatStates.Active)
        {
            entity.CanMove = false;
            movement.CanDash = false;
            //Make copy of enemy manager's list of enemies
            hittableEnemies = new List<GameObject>(enemyMan.Enemies);
            //Check to see if we need to activate another hitbox
            switch (currentAttack)
            {
                case Attacks.Slash:
                    // --do AABB for box 1--
                    AttackRoutine(slHB1, slDamage, slKnockbackTime, slKnockbackSpeed);
                    GameObject tempObjBox1 = Instantiate(tempHitboxObj[0] as GameObject, this.transform);
                    tempObjBox1.transform.localPosition = new Vector3(slHB1.center.x * hitBoxDirectionMove, slHB1.center.y, 0);
                    if (attackTime > (slStartup + slHB2FirstActiveFrame) * Helper.frame)
                    {
                        // --do AABB for box 2--
                        AttackRoutine(slHB2, slDamage, slKnockbackTime, slKnockbackSpeed);
                        GameObject tempObjBox2 = Instantiate(tempHitboxObj[1] as GameObject, this.transform);
                        tempObjBox2.transform.localPosition = new Vector3(slHB2.center.x * hitBoxDirectionMove, slHB2.center.y, 0);
                    }
                    if (attackTime > (slStartup + slHB3FirstActiveFrame) * Helper.frame)
                    {
                        // --do AABB for box 3--
                        AttackRoutine(slHB3, slDamage, slKnockbackTime, slKnockbackSpeed);
                        GameObject tempObjBox3 = Instantiate(tempHitboxObj[2] as GameObject, this.transform);
                        tempObjBox3.transform.localPosition = new Vector3(slHB3.center.x * hitBoxDirectionMove, slHB3.center.y, 0);
                    }
                    if (attackTime > (slStartup + slActive) * Helper.frame)
                        combatState = CombatStates.Recovery;
                    break;
                case Attacks.Thrust:
                    // --do AABB for box 1--
                    AttackRoutine(thHB1, thDamage, thKnockbackTime, thKnockbackSpeed);
                    GameObject tempObjBox1th = Instantiate(tempHitboxObj[3] as GameObject, this.transform);
                    tempObjBox1th.transform.localPosition = new Vector3(thHB1.center.x * hitBoxDirectionMove, thHB1.center.y, 0);//original
                    //tempObjBox1th.transform.localPosition = new Vector3((thHB1.x - (thHB1.width / 2f)) * hitBoxDirectionMove, thHB1.y - (thHB1.height / 2f), 0);
                    //tempObjBox1th.transform.localPosition = Helper.Vec2ToVec3(Helper.Midpoint(thHB1.min, thHB1.max));
                    //tempObjBox1th.transform.localPosition = Helper.Vec2ToVec3(thHB1.min);
                    if (attackTime > (thStartup + thHB2FirstActiveFrame) * Helper.frame)
                    {
                        // --do AABB for box 2--
                        AttackRoutine(thHB2, thDamage, thKnockbackTime, thKnockbackSpeed);
                        GameObject tempObjBox2th = Instantiate(tempHitboxObj[4] as GameObject, this.transform);
                        tempObjBox2th.transform.localPosition = new Vector3(thHB2.center.x * hitBoxDirectionMove, thHB2.center.y, 0);
                    }
                    if (attackTime > (thStartup + thHB3FirstActiveFrame) * Helper.frame)
                    {
                        // --do AABB for box 3--
                        AttackRoutine(thHB3, thDamage, thKnockbackTime, thKnockbackSpeed);
                        GameObject tempObjBox3th = Instantiate(tempHitboxObj[5] as GameObject, this.transform);
                        tempObjBox3th.transform.localPosition = new Vector3(thHB3.center.x * hitBoxDirectionMove, thHB3.center.y, 0);
                    }
                    if (attackTime > (thStartup + thActive) * Helper.frame)
                        combatState = CombatStates.Recovery;
                    break;
                case Attacks.Shine:
                    //Test shine method for each hitbox in the shine
                    for (int i = 0; i < shHB.Length; ++i) 
                    {
                        Shine(shHB[i]);
                    }
                    if (attackTime > (shStartup + shActive) * Helper.frame)
                        combatState = CombatStates.Recovery;
                    break;
            }
        }
        if (combatState == CombatStates.Recovery)
        {
            switch (currentAttack)
            {
                case Attacks.Slash:
                    if (attackTime > (slStartup + slActive + slRecovery) * Helper.frame)
                    {
                        combatState = CombatStates.None;
                        canAttack = true;
                        entity.CanMove = true;
                        entity.Speed *= 1.5f;
                        movement.CanDash = true;
                        attackTime = 0f;
                        consecSlashCount = 0;
                    }
                    break;
                case Attacks.Thrust:
                    if (attackTime > (thStartup + thActive + thRecovery) * Helper.frame)
                    {
                        combatState = CombatStates.None;
                        canAttack = true;
                        entity.CanMove = true;
                        movement.CanDash = true;
                        attackTime = 0f;
                    }
                    break;
                case Attacks.Shine:
                    if (attackTime > (shStartup + shActive + shRecovery) * Helper.frame)
                    {
                        combatState = CombatStates.None;
                        canAttack = true;
                        entity.CanMove = true;
                        movement.CanDash = true;
                        attackTime = 0f;
                    }
                    break;
                case Attacks.None:
                    combatState = CombatStates.None;
                    canAttack = true;
                    entity.CanMove = true;
                    movement.CanDash = true;
                    attackTime = 0f;
                    break;
            }
        }
    }

    /// <summary>
    /// Used to render hitboxes in the editor, remove when game is published
    /// </summary>
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        if (slEditorHitboxes)
        {
            Gizmos.color = Color.red;
            Helper.DebugDrawRect(this.transform.position, slHB1);
            Gizmos.color = Color.green;
            Helper.DebugDrawRect(this.transform.position, slHB2);
            Gizmos.color = Color.blue;
            Helper.DebugDrawRect(this.transform.position, slHB3);
        }
        if (thEditorHitboxes)
        {
            Gizmos.color = Color.red;
            Helper.DebugDrawRect(this.transform.position, thHB1);
            Gizmos.color = Color.green;
            Helper.DebugDrawRect(this.transform.position, thHB2);
            Gizmos.color = Color.blue;
            Helper.DebugDrawRect(this.transform.position, thHB3);
        }
        if (shEditorHitboxes)
        {
            for (int i = 0; i < shHB.Length; ++i)
            {
                Helper.DebugDrawRect(this.transform.position, shHB[i]);
            }
        }
    }

    
    #endregion

    #region Custom Methods
    /// <summary>
    /// Simply deals damage to an enemy
    /// </summary>
    /// <param name="e">Reference of the enemy to damage</param>
    /// <param name="damage">Amount of damage to dish out</param>
    void Attack(Enemy e, float damage)
    {
        if (e.CanTakeDamage)
        {
            e.TakeDamage(damage, this.currentAttack);
        }
    }

    /// <summary>
    /// Used to Cancel any ongoing attack and dash frames
    /// </summary>
    void Cancel()
    {
        // Set combat state to Recovery
        combatState = CombatStates.Recovery;
        // Set attackTime to the max float value to immediately end recovery frames
        attackTime = float.MaxValue;
        movement.dashTime = .00000001f; // incredibly small positive number so the next frame will end the dash
    }

    /// <summary>
    /// Stops all progression of time to give weight to hits
    /// </summary>
    /// <param name="lengthInframes"></param>
    void HitStop(float lengthInframes)
    {
        while (lengthInframes >= 0)
        {
            lengthInframes -= Time.deltaTime;
            // Ssssstop
            Time.timeScale = 0;
        }

        // reset time scale to normal
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Checks to see if an enemy can be knocked back, and if so, knocks him back
    /// </summary>
    /// <param name="e">Enemy to knockback</param>
    /// <param name="time">Duration of the knockback</param>
    /// <param name="speed">Velocity of the knockback</param>
    void Knockback(Enemy e, float time, Vector2 speed)
    {
        if (e.CanKnockback)
            e.RecieveKnockback(time, speed);
    }

    /// <summary>
    /// Tests collisions with all enemies
    /// </summary>
    /// <param name="hitbox">Hitbox to compare with enemies</param>
    /// <returns>A list of all enemies collided with</returns>
    protected List<Enemy> CheckCollisions(Rect hitbox)
    {
        List<Enemy> hit = new List<Enemy>();
        Rect newHB = hitbox;
        if (hitBoxDirectionMove < 0)
            newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
        for (int i = 0; i < hittableEnemies.Count; ++i)
        {
            Enemy e = hittableEnemies[i].GetComponent<Enemy>();
            if (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), e.HitBoxRect))
            {
                hit.Add(e);
                //Remove enemies we hit so we don't hit them again this frame
                hittableEnemies.RemoveAt(i);
                --i;
                //e.TakeDamage(slDamage);
            }
        }
        return hit;
    }

    /// <summary>
    /// Checks for collisions, attacks enemies, and knocks them back
    /// Basically compresses the three steps into one method to save space 
    /// </summary>
    /// <param name="hitbox">Hitbox of the attack</param>
    /// <param name="damage">Amount of damage to deal</param>
    /// <param name="knockbackTime">Duration of knockback</param>
    /// <param name="knockbackSpeed">Velocity of knockback</param>
    protected void AttackRoutine(Rect hitbox, float damage, float knockbackTime, float knockbackSpeed)
    {
        hitEnemies = CheckCollisions(hitbox);
        if (hitEnemies.Count > 0)
        {
            Vector3 v;
            for (int i = 0; i < hitEnemies.Count; ++i)
            {
                v = hitEnemies[i].transform.position - this.transform.position;
                Attack(hitEnemies[i], damage);
                Knockback(hitEnemies[i], knockbackTime, new Vector2(v.x, v.y) * knockbackSpeed);
            }
        }
    }

    /// <summary>
    /// Performs the shine and resolves hitbox collisions
    /// </summary>
    /// <param name="hitbox">Hitbox of the shine to test</param>
    protected void Shine(Rect hitbox)
    {
        ///TODO: collision detection for projectiles, once projectiles are implemented
        ///TODO: enemy attack countering, once enemy attacks are implemented
        ///
        //Get enemy active hitboxes
        //List<Rect> enemyHitboxes = enemyMan.EnemyAttackHitboxes;
        Rect[] hitboxes = new Rect[3];
        List<Enemy> enemies = new List<Enemy>();
        for (int i = 0; i < enemyMan.encounterEnemies.Count; ++i)
            enemies.Add(enemyMan.encounterEnemies[i].GetComponent<Enemy>());
        //Update hitbox direction
        Rect newHB = hitbox;
        if (hitBoxDirectionMove < 0)
            newHB = new Rect((hitbox.x + hitbox.width) * -1, hitbox.y, hitbox.width, hitbox.height);
        //Loop through 'em
        for (int i = 0; i < enemies.Count; ++i)
        {
            //Check to see if they're attacking
            if(enemies[i].isAttacking)
            {
                //Check for collision
                hitboxes[0] = enemies[i].atHB1;
                hitboxes[1] = enemies[i].atHB2;
                hitboxes[2] = enemies[i].atHB3;
                for (int j = 0; j < 3; ++j)
                {
                    if (Helper.AABB(Helper.LocalToWorldRect(newHB, this.transform.position), Helper.LocalToWorldRect(hitboxes[j], enemies[i].transform.position)))
                    {
                        Debug.Log("Shine connected");
                        enemies[i].BreakGuard(6f);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Makes the player lose HP
    /// </summary>
    /// <param name="damage">Damage (Positive; this method will subtract)</param>
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Debug.Log("Player died");
            //TODO: Death stuff
        }
    }
#endregion
}