using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {

    //Health points
    public float hp;
    //When set to false the enemy manager will destroy&remove the object
    protected bool alive;
    public bool Alive { get { return alive; } }
    //Reference to the hitbox
    protected BoxCollider2D hitBox;
    public Rect HitBoxRect { get { return new Rect(new Vector2(hitBox.bounds.center.x, hitBox.bounds.center.y), new Vector2(hitBox.bounds.extents.x, hitBox.bounds.extents.y)); } }
    //For the hit response visual... still a temporary solution???
    protected float hitFlashTimer;
    public Material defaultMat, hitflashMat;
    //Prevents taking damage from multiple sources in the same (very small) time frame
    protected float damageTimer = 0f;
    public bool CanTakeDamage { get { return (damageTimer <= 0f); } }

	void Start () {
        alive = true;
        hitBox = this.GetComponent<BoxCollider2D>();
        hitFlashTimer = 0f;
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
}