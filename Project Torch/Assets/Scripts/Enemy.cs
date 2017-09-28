using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic enemy class
/// Right now it just stands there, takes damage, and eventually dies
/// TODO: basic attack functions
/// 
/// Stuff to know:
///     Properties:
///         bool Alive - whether or not the enemy manager should destroy it
///         bool CanTakeDamage - whether or not the enemy 
/// </summary>
public class Enemy : MonoBehaviour {
    #region Private Fields
    //When set to false the enemy manager will destroy&remove the object
    protected bool alive;
    //Reference to the hitbox
    protected BoxCollider2D hitBox;
    //For the hit response visual... still a temporary solution???
    protected float hitFlashTimer;
    //Prevents taking damage from multiple sources in the same (very small) time frame
    protected float damageTimer = 0f;
    //Reference to the entity class
    protected Entity entity;

    ///List<Rect> hitboxesCollidedWith  //Resume from here... make a better hitbox delay function
    #endregion
    #region Public Fields
    //Health points
    public float hp;
    //Materials used for flashing when hit... //DEBUG//
    public Material defaultMat, hitflashMat;
    #endregion
    #region Properties
    public bool Alive { get { return alive; } }
    public bool CanTakeDamage { get { return (damageTimer <= 0f); } }
    public Rect HitBoxRect { get { return new Rect(new Vector2(hitBox.bounds.center.x, hitBox.bounds.center.y), new Vector2(hitBox.bounds.extents.x, hitBox.bounds.extents.y)); } }
    #endregion
    #region Unity Methods
    void Start () {
        alive = true;
        hitBox = this.GetComponent<BoxCollider2D>();
        hitFlashTimer = 0f;
        entity = this.GetComponent<Entity>();
	}
	
	void Update () {
        //Hit flash
        if (hitFlashTimer > 0f)
        {
            hitFlashTimer -= Time.deltaTime;
            if (Time.frameCount % 8 > 4)
                this.GetComponent<MeshRenderer>().material = hitflashMat;
            else this.GetComponent<MeshRenderer>().material = defaultMat;
        }
        else this.GetComponent<MeshRenderer>().material = defaultMat;

        //Damage time limiter
        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
	}
    #endregion
    #region Custom Methods
    /// <summary>
    /// Deals HP damage to this enemy
    /// </summary>
    /// <param name="damage">Amout of HP damage the enemy will take</param>
    public void TakeDamage(float damage)
    {
        hp -= damage;
        hitFlashTimer = 0.6f;
        damageTimer = 0.2f;
        if (hp < 0)
            alive = false;
    }

    //public bool CanTakeDamageFromHitbox()
#endregion
}