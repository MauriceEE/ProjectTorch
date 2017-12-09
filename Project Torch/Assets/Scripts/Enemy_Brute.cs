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
    protected float guardTime;
    #region Unity Defaults
    protected void Start()
    {
        soundEffect_Attack = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanBruteAttack : SoundManager.SoundEffects.EnemyShadowBruteAttack;
        soundEffect_Dash = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanBruteDash : SoundManager.SoundEffects.EnemyShadowBruteDash;
        soundEffect_Death = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanBruteDeath : SoundManager.SoundEffects.EnemyShadowBruteDeath;
        soundEffect_Hit = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanBruteHit : SoundManager.SoundEffects.EnemyShadowBruteHit;
        soundEffect_Walk = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanBruteWalk : SoundManager.SoundEffects.EnemyShadowBruteWalk;
        base.guardStacks += 1;
        base.guarding = true;
        if (base.guarding) baseColor = Color.yellow;
        guardTime = 0;
    }
    #endregion
    #region Override Methods
    public override void BreakGuard(float knockbackMultiplier)
    {
        base.BreakGuard(knockbackMultiplier);
    }
    protected override void React()
    {
        base.maxGuardStacks = 1;
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
            this.entity.SpeedModifier *= 0.5f;//NOTE: This was changed to use multiplication to test the new speed system @ 11/8
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
            entity.SpeedModifier *= dashSpeed;
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
        guardTime = 0; // reset guard time on hit
        base.TakeDamage(damage, attackType);
    }
    protected override void UpdateColor()
    {
        base.UpdateColor();
        if (!lit && faction == EnemyFaction.Shadow && base.guardStacks <= 0 && enemyState != EnemyStates.ApproachingToAttack && enemyState != EnemyStates.Attacking) baseColor.a = .75f;
        this.GetComponent<SpriteRenderer>().color = baseColor;
    }
    protected override void UpdateCombatState()
    {
        base.UpdateCombatState();
        //play attack sound if attacking
		if (!attackAudioPlayed && combatState == CombatStates.Active) {
            SoundManager.PlaySound(soundEffect_Attack, this.gameObject);
            attackAudioPlayed = true;
            
        }
        if(combatState == CombatStates.Startup) {
            animator.Play("Slash");
        }
        // if no guard stacks, update guardTime
        if (guardStacks <= 0) guardTime += Time.deltaTime;
        else guardTime = 0;

        switch(faction)
        {
            case EnemyFaction.Human:
                if (guardTime >= 2.5f) // after 2 seconds, put up guard again
                {
                    Mathf.Clamp(guardStacks++, 0, 1);
                    guarding = true;
                }
                break;
            case EnemyFaction.Shadow:
                if (guardTime >= 4.5f) // after 4 seconds, put up guard again
                {
                    Mathf.Clamp(guardStacks++, 0, 1);
                    guarding = true;
                }
                break;
        }

        if (combatState == CombatStates.Recovery || combatState == CombatStates.None) attackAudioPlayed = false;
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

