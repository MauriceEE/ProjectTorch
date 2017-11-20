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
public class Enemy_Spearman: Enemy {

    protected bool triedToDash = false;
    #region Unity Defaults
    protected void Start()
    {
        soundEffect_Attack = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanSpearmanAttack : SoundManager.SoundEffects.EnemyShadowSpearmanAttack;
        soundEffect_Dash = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanSpearmanDash : SoundManager.SoundEffects.EnemyShadowSpearmanDash;
        soundEffect_Death = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanSpearmanDeath : SoundManager.SoundEffects.EnemyShadowSpearmanDeath;
        soundEffect_Hit = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanSpearmanHit : SoundManager.SoundEffects.EnemyShadowSpearmanHit;
        soundEffect_Walk = (faction == EnemyFaction.Human) ? SoundManager.SoundEffects.EnemyHumanSpearmanWalk : SoundManager.SoundEffects.EnemyShadowSpearmanWalk;
    }
    #endregion
    #region Override Methods
    public override void BreakGuard(float knockbackMultiplier)
    {
        base.BreakGuard(knockbackMultiplier);
    }
    protected override void React()
    {
        irwType = "counterattack";
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
        base.UpdateCombatState();
        //play attack sound if attacking
		if (!attackAudioPlayed && combatState == CombatStates.Active) {
			if (faction == Enemy.EnemyFaction.Human) AkSoundEngine.PostEvent ("Human_Spearman_Attack", gameObject);

			if (faction == Enemy.EnemyFaction.Shadow) AkSoundEngine.PostEvent ("Shadow_Spearman_Attack", gameObject);

            attackAudioPlayed = true;
		}
        if (combatState == CombatStates.Recovery || combatState == CombatStates.None) attackAudioPlayed = false;

        if(!triedToDash && combatState == CombatStates.Recovery)
        {
            int dashTryResult = Random.Range(1, 4);
            triedToDash = true;
            //Debug.Log("Dash try result: " + dashTryResult);
            if (dashTryResult != 0) Dodge();
        }
        if (combatState == CombatStates.None) triedToDash = false;
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
