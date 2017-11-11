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
				AkSoundEngine.PostEvent ("Human_Basic_Attack", gameObject);

			if (faction == Enemy.EnemyFaction.Shadow)
				AkSoundEngine.PostEvent ("Shadow_Basic_Attack", gameObject);
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
