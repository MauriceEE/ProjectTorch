using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This enemy subclass should be equipped on the guards
///     that escort the princess in her level
/// </summary>
public class Enemy_PrincessEscort : Enemy {

    #region Enums
    public enum EscortType
    {
        Stationary,
        Captain
    }
    #endregion

    #region Private Fields
    [Header("ENEMY_PRINCESSESCORT")]
    public EscortType type;
    public float escortingSpeedMultiplier;
    #endregion

    #region Override Methods
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
        /*
        if (guardStacks == 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                alive = false;
                return;
            }
            CancelOrHitStun(true);
            hitFlashTimer = 0.6f;
            damageTimer = 0.2f;
        }
        if (enemyState == EnemyStates.Stunned || enemyState == EnemyStates.Knockback)
        {
            stunTime = 0f;
            ResetCombatStates();
            Debug.Log("attack after guard break @ " + Time.fixedTime);
        }
        if (!inEncounter)
        {
            if (encounter)
            {
                encounter.GetComponent<Encounter>().StartEncounter(this.faction);
            }
            else
            {
                Debug.Log("Encounter object not yet assigned to this enemy!");
            }
        }
        if (guardStacks > 0)
        {
            if (attackType == PlayerCombat.Attacks.Thrust)
                if (--guardStacks <= 0)
                {
                    BreakGuard(2f);
                    guarding = false;
                }
            if (attackType == PlayerCombat.Attacks.Slash)
            {
                Debug.Log("TODO: REBOUND THINGEY");
            }
        }
        if (!guarding && !dodging && !counterattacking && enemyState != EnemyStates.Stunned && enemyState != EnemyStates.Knockback)
            React();
            */
        base.TakeDamage(damage, attackType);
        encounter.GetComponent<Encounter>().Range *= 100f;
    }
    protected override void UpdateCombatState()
    {
        base.UpdateCombatState();
    }
    protected override void UpdateEnemyState()
    {
        switch (enemyState)
        {
            case EnemyStates.Idle:
                isAttacking = false;
                //The captain should just keep moving right if not in battle
                if (type == EscortType.Captain)
                {
                    moveTarget = new Vector2(10000f, this.transform.position.y);
                    SeekTarget();
                    //Move slower so it's fair
                    entity.SpeedModifier = escortingSpeedMultiplier;
                }
                if (inEncounter)
                    RequestMoveTarget();//Don't do nothing if you're in an encounter
                break;
            case EnemyStates.Attacking:
                elapsedApproachTime = 0f;
                attackRange = ogAttackRange;
                isAttacking = true;
                attackTime += Time.deltaTime;
                entity.Displacement = Vector2.zero;//Can't move while attacking
                break;
            case EnemyStates.ApproachingToAttack:
                isAttacking = false; 
                if (maxVelocity <= (3 * ogMaxVelocity))
                    maxVelocity += (Time.deltaTime / 15);
                elapsedApproachTime += Time.deltaTime;
                if (elapsedApproachTime >= 1) attackRange = .8f;
                if (elapsedApproachTime > 3)
                {
                    CancelOrHitStun(false);
                    enemyState = EnemyStates.ReturningFromAttack;
                }
                moveTarget = attackTarget.transform.position;
                SeekTarget();
                if ((this.transform.position - Helper.Vec2ToVec3(moveTarget)).sqrMagnitude <= Mathf.Pow(attackRange, 2))
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
                //TODO: They shouldn't ever stop attacking the player here?
                SeekTarget();
                break;
            case EnemyStates.SurroundingPlayer:
                maxVelocity = ogMaxVelocity / 2;
                SeekTarget();
                break;
            case EnemyStates.Dodging:
                SeekTarget();
                dashTime -= Time.deltaTime;
                if (dashTime <= 0f)
                {
                    dodging = false;
                    enemyState = EnemyStates.ReturningFromAttack;
                    entity.SpeedModifier = 1f;
                    invincible = false;
                }
                else
                {
                    invincible = (dashTime > (dashFrames - dashIFrames) * Helper.frame);
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
    #endregion
}
