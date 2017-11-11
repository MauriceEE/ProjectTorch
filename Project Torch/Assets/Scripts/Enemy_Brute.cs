using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is a sample of what an enemy subclass can do
/// Specifically, all the methods you see here are override-able
///     and as such all you really need to do to make an enemy subclass
///     is to re-implement these methods. The base abstract class will
///     handle everything else
/// </summary>

public class Enemy_Brute : Enemy
{
    #region Override Methods
    public override void BreakGuard(float knockbackMultiplier)
    {
        base.BreakGuard(knockbackMultiplier);
    }
    protected override void React()
    {
        base.maxGuardStacks = 3;
        //Generate random chance between 0% and 100% (represented as 0 to 100)
        float rand = Random.Range(0f, 100f);
        //Check to see if we fell within guard's percent chance
        if (rand < guardChance)
        {
            //Enter guarding state
            guarding = true;
            //Increment stacks but stay within bounds
            guardStacks += 2;
            if (guardStacks > maxGuardStacks)
                guardStacks = maxGuardStacks;
            //Halve speed
            this.entity.SpeedModifier = 0.5f;
        }
        else if (rand < guardChance + counterAttackChance)
        {
            // Ask Encounter Manager if it can attack
            if (GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>().CanEnemiesAttackPlayer())
            {
                //Enter counterattack state
                counterattacking = true;
                //Attack the player
                MoveToAttack(player);
            }
        }
        else if (rand < guardChance + counterAttackChance + dodgeChance)
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
            entity.SpeedModifier = dashSpeed;
            //Remove from occupancy grid
            RequestRemoveFromEncounterGrid();
        }
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
        base.UpdateCombatState();

		//play attack sound if attacking
		if (combatState == CombatStates.Active) {
			if (faction == Enemy.EnemyFaction.Human)
				AkSoundEngine.PostEvent ("Human_Brute_Attack", gameObject);

			if (faction == Enemy.EnemyFaction.Shadow)
				AkSoundEngine.PostEvent ("Shadow_Brute_Attack", gameObject);
		}
    }
    protected override void UpdateEnemyState()
    {
        base.UpdateEnemyState();
    }
    protected override void UpdateKnockback()
    {
        base.UpdateKnockback();
    }
    #endregion
}

