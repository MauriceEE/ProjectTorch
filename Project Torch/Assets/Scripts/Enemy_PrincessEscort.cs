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

    #region Fields
    protected FlagManager flagMan;
    [Header("ENEMY_PRINCESSESCORT")]
    public EscortType type;
    public float escortingSpeedMultiplier;
    public float playerAggroDist;
    #endregion

    #region Override Methods
    protected override void Awake()
    {
        base.Awake();
        flagMan = GameObject.Find("FlagManagerGO").GetComponent<FlagManager>();
    }
    protected void Start()
    {
        
    }
    protected override void Update()
    {
        base.Update();
        //Automatically start an encounter if within range of the player
        if (!inEncounter)
        {
            if ((this.transform.position - player.transform.position).sqrMagnitude <= playerAggroDist * playerAggroDist)
            {
                encounter.GetComponent<Encounter>().StartEncounter(this.faction);
                //Also prevents the player from fleeing
                //  This line should stay after so that the encounter only gathers enemies within its range,
                //  but then gets its range multiplied so the player can't escape
                encounter.GetComponent<Encounter>().Range *= 10f;
            }
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
    protected override void UpdateCombatState()
    {
        base.UpdateCombatState();
        //play attack sound if attacking
		if (combatState == CombatStates.Active) {
			if (faction == Enemy.EnemyFaction.Human)
            { 
				AkSoundEngine.PostEvent ("Human_PrincessEscort_Attack", gameObject);
                animator.Play("Slash");
            }

			if (faction == Enemy.EnemyFaction.Shadow)
				AkSoundEngine.PostEvent ("Shadow_PrincessEscort_Attack", gameObject);
		}
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
                    entity.SpeedModifier *= escortingSpeedMultiplier;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
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
                    entity.SpeedModifier *= 1f;//NOTE: This was changed from "= 1f" to "*= 1f" to test the new speed system @ 11/8
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
    public override void StartEncounter()
    {
        base.StartEncounter();
        //Because they were moving slower before the encounter (if they were the captain)
        entity.SpeedModifier *= 1f;//NOTE: This was changed from "= 1f" to "*= 1f" to test the new speed system @ 11/8
        //Make the princess scared
        GameObject.Find("Princess").GetComponent<Princess>().State = Princess.PrincessStates.CoweringInFear;
    }
    #endregion
}
