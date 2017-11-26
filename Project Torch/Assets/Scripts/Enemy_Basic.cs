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
public class Enemy_Basic : Enemy {
    #region Unity Defaults
    protected void Start()
    {
        //Assign sound effects based on whether human or shadow
        soundEffect_Attack = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanAttack : SoundManager.SoundEffects.EnemyShadowAttack;
        soundEffect_Dash = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanDash : SoundManager.SoundEffects.EnemyShadowDash;
        soundEffect_Death = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanDeath : SoundManager.SoundEffects.EnemyShadowDeath;
        soundEffect_Hit = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanHit : SoundManager.SoundEffects.EnemyShadowHit;
        soundEffect_Walk = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanWalk : SoundManager.SoundEffects.EnemyShadowWalk;
    }
    #endregion
    #region Override Methods
    public override void BreakGuard(float knockbackMultiplier)
    {
        base.BreakGuard(knockbackMultiplier);
    }
    protected override void React()
    {
        irwType = "dodge";
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
        if (!lit && faction == EnemyFaction.Shadow && enemyState == EnemyStates.ApproachingToAttack) baseColor.a = .5f;
        this.GetComponent<SpriteRenderer>().color = baseColor;
    }
    protected override void UpdateCombatState()
    {
        base.UpdateCombatState();
        //play attack sound if attacking
		if (!attackAudioPlayed && combatState == CombatStates.Active) {
            animator.Play("Slash");
            SoundManager.PlaySound(soundEffect_Attack, this.gameObject);
            attackAudioPlayed = true;
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
