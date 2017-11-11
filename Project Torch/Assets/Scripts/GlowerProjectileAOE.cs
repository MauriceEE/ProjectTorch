using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The AOE blast left from collision of the glower projectile
/// Applies a slow debuff to anyone of the opposing side within range
/// NOTE: Public values must be set in the inspector!!
/// </summary>
public class GlowerProjectileAOE : MonoBehaviour {
    #region Public Fields
    public float lifetime;
    public float activeDuration;
    public float slowDuration;
    public float radius;
    #endregion
    #region Private Fields
    protected float timeActive;
    protected bool alliedWithPlayer;
    protected EnemyManager enemyMan;
    protected PlayerCombat player;
    #endregion
    #region Properties
    public bool AlliedWithPlayer { get { return alliedWithPlayer; } set { alliedWithPlayer = value; } }
    #endregion
    #region Unity Defaults
    void Start () {
        timeActive = 0f;
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
	}
	void Update () {
        //Check collisions only if dangerous
        if (timeActive < activeDuration) 
        {
            if (alliedWithPlayer)
                CheckCollisionWithEnemies();
            else
                CheckCollisionWithPlayer();
        }
        //Update time
        timeActive += Time.deltaTime;
        //Kill if over
        if (timeActive > lifetime)
            Destroy(this.gameObject);
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Checks collisions with the player and applies the slow effect if within range
    /// </summary>
    protected void CheckCollisionWithPlayer()
    {
        //Check dist to player... need to also consider the player's hitbox
        if ((this.transform.position - player.transform.position).sqrMagnitude < (radius * radius) + (player.HitBoxRect.width * player.HitBoxRect.width)
            && !player.GetComponent<Entity>().ContainsStatusEffect(StatusEffectManager.Debuffs.GlowerSlow))
            player.GetComponent<Entity>().ApplyNewStatusEffect(new StatusEffect(
                slowDuration,
                StatusEffectManager.Buffs.NONE,
                StatusEffectManager.Debuffs.GlowerSlow,
                StatusEffectManager.StatusEffects.NONE));
    }
    /// <summary>
    /// Applies status effects to enemies within range
    /// </summary>
    protected void CheckCollisionWithEnemies()
    {
        for (int i = 0; i < enemyMan.encounterEnemies.Count; ++i)
        {
            //deer lord this if statement is a sin
            //Maybe take out the top line here? Currently it prevents enemies allied with you from getting slowed but it might look better if they do
            if (!enemyMan.encounterEnemies[i].GetComponent<Enemy>().AlliedWithPlayer &&
                (this.transform.position - enemyMan.encounterEnemies[i].transform.position).sqrMagnitude < 
                (radius * radius) + (enemyMan.encounterEnemies[i].GetComponent<Entity>().HitBoxRect.width * enemyMan.encounterEnemies[i].GetComponent<Entity>().HitBoxRect.width)
            && !enemyMan.encounterEnemies[i].GetComponent<Entity>().ContainsStatusEffect(StatusEffectManager.Debuffs.GlowerSlow))
                enemyMan.encounterEnemies[i].GetComponent<Entity>().ApplyNewStatusEffect(new StatusEffect(
                    slowDuration,
                    StatusEffectManager.Buffs.NONE,
                    StatusEffectManager.Debuffs.GlowerSlow,
                    StatusEffectManager.StatusEffects.NONE));
        }
    }
    #endregion
}