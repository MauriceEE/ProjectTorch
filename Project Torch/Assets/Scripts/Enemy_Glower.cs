using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The Glower enemy subclass
/// Designed to shoot a projectile at the player and pester them
/// </summary>
public class Enemy_Glower : Enemy {
    #region Public Fields
    [Header("GLOWER Fields")]
    public GameObject projectile;
    public float projectileMaxSpeed;
    public float projectileRadius;
    public float projectileHomingForce;
    public float projectileLifetime;
    public float moveToAttackTime;
    public float shinedMaxSpeedMultiplierIncrement;
    public float shinedHomingForceMultiplierIncrement;
    public int shinedMaxIncrements;
    #endregion
    #region Private Fields
    protected PlayerCombat playerCombat;
    protected int shinedIncrements;
    protected float shinedCooldown;
    #endregion
    #region Override Methods
    protected void Start()
    {
        playerCombat = player.GetComponent<PlayerCombat>();
        soundEffect_Attack = SoundManager.SoundEffects.EnemyShadowGlowerAttack;
        soundEffect_Dash = SoundManager.SoundEffects.EnemyShadowGlowerDash;
        soundEffect_Death = SoundManager.SoundEffects.EnemyShadowGlowerDeath;
        soundEffect_Hit = SoundManager.SoundEffects.EnemyShadowGlowerHit;
        soundEffect_Walk = SoundManager.SoundEffects.EnemyShadowGlowerWalk;

        base.enemyLight = gameObject.AddComponent<Light>();
        base.enemyLight.range = 3.5f;
        base.enemyLight.intensity = 12;
        base.enemyLight.color = new Color((155 / 255), (0 / 255), (155 / 255));
        base.lightTimer = 0;
        base.glower = true;
    }
    protected override void Update()
    {
        // stay a purplish hue, Ponyboy
        if (base.lightTimer <= 0)
        {
            //base.enemyLight = gameObject.AddComponent<Light>();
            //base.enemyLight.range = 3.5f;
            base.enemyLight.intensity = 3;
            base.enemyLight.color = Color.magenta;
        }

        //BASE UPDATE
        base.Update();
        
        //Check shine hit
        if (shinedCooldown > 0 && playerCombat.CurrentAttack == PlayerCombat.Attacks.Shine) 
        {
            List<Rect> shineHitboxes = playerCombat.GetActiveAttackHitboxes();
            for (int i = 0; i < shineHitboxes.Count; ++i)
            {
                if (Helper.AABB(this.HitBoxRect, shineHitboxes[i]))
                {
                    //Increment times shined but stay within range
                    if (++shinedIncrements > shinedMaxIncrements)
                        shinedIncrements = shinedMaxIncrements;
                    //Cooldown for 1 sec so we don't register this multiple times
                    shinedCooldown = 1f;
                }
            }
            shinedCooldown -= Time.deltaTime;
        }
    }
    public override void BreakGuard(float knockbackMultiplier)
    {
        base.BreakGuard(knockbackMultiplier);
    }
    protected override void React()
    {
        base.React();
    }
    protected override void CancelOrHitStun(bool hitstun)
    {
        base.CancelOrHitStun(hitstun);
    }
    public override void RecieveKnockback(float time, Vector2 speed)
    {
        base.RecieveKnockback(time, speed);
    }
    public override void TakeDamage(float damage, PlayerCombat.Attacks attackType)
    {
        base.TakeDamage(damage, attackType);
    }
    protected override void UpdateColor()
    {
        base.UpdateColor();
    }
    protected override void UpdateCombatState()
    {
        switch (combatState)
        {
            case CombatStates.Active:
                entity.CanMove = false;
                //Projectile shooting
                //Change target depending on whether or not allied with player
                if (!alliedWithPlayer)
                {
                    GlowerProjectile shot = Instantiate(projectile).GetComponent<GlowerProjectile>();
                    shot.Setup(this.gameObject, projectileMaxSpeed + (projectileMaxSpeed * shinedMaxSpeedMultiplierIncrement * (float)shinedIncrements), 
                        projectileRadius, projectileHomingForce + (projectileHomingForce * shinedHomingForceMultiplierIncrement * (float)shinedIncrements), 
                        projectileLifetime);
                    shot.transform.position = this.transform.position;
                    shinedIncrements = 0;
                    SoundManager.PlaySound(soundEffect_Attack, this.gameObject);
                }
                else
                {
                    GlowerProjectile shot = Instantiate(projectile).GetComponent<GlowerProjectile>();
                    shot.Setup(this.gameObject, projectileMaxSpeed + (projectileMaxSpeed * shinedMaxSpeedMultiplierIncrement * (float)shinedIncrements), 
                        projectileRadius, projectileHomingForce + (projectileHomingForce * shinedHomingForceMultiplierIncrement * (float)shinedIncrements), 
                        projectileLifetime);
                    shot.transform.position = this.transform.position;
                    shinedIncrements = 0;
                    shot.AlliedWithPlayer = true;
                    shot.Target = enemyMan.GetNewAttackTarget(this.faction);
                    SoundManager.PlaySound(soundEffect_Attack, this.gameObject);
                }
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
    protected override void UpdateEnemyState()
    {
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
                //Update move target
                moveTarget = attackTarget.transform.position;
                //Move towards target
                SeekTarget();
                //See if you're within range
                if (elapsedApproachTime > moveToAttackTime) 
                {
                    //Change states accordingly
                    combatState = CombatStates.Startup;
                    enemyState = EnemyStates.Attacking;
                    attackTime = 0f;
                }
                attackRange = ogAttackRange;
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
                    enemyState = EnemyStates.Idle;
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
        }
    }
    protected override void UpdateKnockback()
    {
        base.UpdateKnockback();
    }
    #endregion
}
