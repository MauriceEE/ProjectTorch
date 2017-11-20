using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The projectile the glower shoots
/// Tracks its target and explodes when within range
/// NOTE: Requires setting up the AOE explosion object in the inspector
/// </summary>
public class GlowerProjectile : MonoBehaviour {
    #region Public Fields
    public GameObject AOEexplosionObj;
    #endregion
    #region Private Fields
    protected Rect hitboxRect;
    protected GameObject originGlower;
    protected GameObject homingTarget;
    protected Vector2 velocity;
    protected Vector2 acceleration;
    protected PlayerCombat player;
    protected EnemyManager enemyMan;
    protected float lifetime;
    protected float accelerationForceMultiplier;
    protected float maxSpeed;
    protected float hitRadius;
    protected bool alliedWithPlayer;
    #endregion
    #region Properties
    public GameObject Target { get { return homingTarget; } set { homingTarget = value; } }
    public bool AlliedWithPlayer { get { return alliedWithPlayer; } set { alliedWithPlayer = value; } }
    #endregion
    #region Unity Defaults
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerCombat>();
        enemyMan = GameObject.Find("EnemyManagerGO").GetComponent<EnemyManager>();
        homingTarget = player.gameObject;
    }
    void Update () {
        //Update acceleration
        acceleration = Helper.Vec3ToVec2(homingTarget.transform.position - this.transform.position).normalized * accelerationForceMultiplier;
        //Apply to velocity
        velocity += acceleration;
        //Limit velocity
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
        //Update position
        this.transform.position += Helper.Vec2ToVec3(velocity);
        //Check collisions with player attacks if the player is thrusting and you aren't allied with him yet
        if (!alliedWithPlayer && player.CurrentAttack == PlayerCombat.Attacks.Thrust && CheckCollisionsWithPlayerHitboxes())
        {
            //Set as allied with player
            alliedWithPlayer = true;
            //Change homing target
            homingTarget = originGlower;
            //Store current speed
            float speed = velocity.magnitude;
            //Reset velocity to point in direction of enemy
            velocity = Helper.Vec3ToVec2(homingTarget.transform.position - this.transform.position).normalized;
            //Scale up velocity to previous speed
            velocity *= speed;
        }
        //Check dist to targets based on current ally
        if (alliedWithPlayer)
            CheckCollisionsWithEnemies();
        else
            CheckCollisionsWithPlayer();
        //Check if it's been around too long
        if (lifetime < 0)
            Destroy(this.gameObject);
        else
            lifetime -= Time.deltaTime;
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Gets the projectile ready to be shot
    /// SHOULD BE CALLED WHEN INSTANTIATED!!!
    /// </summary>
    /// <param name="_originGlower">The glower that this projectile game from</param>
    /// <param name="_maxSpeed">Maximum speed of the projectile</param>
    /// <param name="_radius">Collision radius of the projectile</param>
    /// <param name="_accelerationForceAmount">Acceleration force (tracking weight)</param>
    /// <param name="_lifetime">Time until the shot dies</param>
    public void Setup(GameObject _originGlower, float _maxSpeed, float _radius, float _accelerationForceAmount, float _lifetime)
    {
        maxSpeed = _maxSpeed;
        hitRadius = _radius;
        accelerationForceMultiplier = _accelerationForceAmount;
        lifetime = _lifetime;
        //Set up hitbox rect
        hitboxRect = new Rect(new Vector2(this.transform.position.x - hitRadius, this.transform.position.y - hitRadius), new Vector2(hitRadius * 2f, hitRadius * 2f));
    }
    /// <summary>
    /// Sees whether or not it's hitting a player attack
    /// </summary>
    /// <returns></returns>
    protected bool CheckCollisionsWithPlayerHitboxes()
    {
        //Get player hitboxes
        List<Rect> hitboxes = player.GetActiveAttackHitboxes();
        //Loop through and if we're colliding (roughly) then return true
        for (int i = 0; i < hitboxes.Count; ++i)
            if (Helper.AABB(hitboxRect, hitboxes[i]))
                return true;
        return false;
    }
    /// <summary>
    /// Checks distance to all encounter enemies not allied with the player and explodes if contact is made
    /// </summary>
    protected void CheckCollisionsWithEnemies()
    {
        for (int i = 0; i < enemyMan.encounterEnemies.Count; ++i)
            if (!enemyMan.encounterEnemies[i].GetComponent<Enemy>().AlliedWithPlayer && 
                (this.transform.position - enemyMan.encounterEnemies[i].transform.position).sqrMagnitude < hitRadius * hitRadius)
                Explode();
    }
    /// <summary>
    /// Checks distance to the player and explodes if contact is made
    /// </summary>
    protected void CheckCollisionsWithPlayer()
    {
        if ((this.transform.position - player.transform.position).sqrMagnitude < hitRadius * hitRadius)
            Explode();
    }
    /// <summary>
    /// Billy Mays here with Kaboom
    /// Kills this object and spawns an explosion object
    /// </summary>
    protected void Explode()
    {
        //Create the AOE blast and let it know whether or not it's allied with the player
        GlowerProjectileAOE aoe = Instantiate(AOEexplosionObj).GetComponent<GlowerProjectileAOE>();
        aoe.AlliedWithPlayer = alliedWithPlayer;
        aoe.transform.position = this.transform.position;
        SoundManager.PlaySound(SoundManager.SoundEffects.EnemyShadowGlowerExplosion, this.gameObject);
        Destroy(this.gameObject);
    }
    #endregion
}