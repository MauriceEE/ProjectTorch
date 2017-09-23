using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {

    public float hp;
    protected bool alive;
    public bool Alive { get { return alive; } }
    protected BoxCollider2D hitBox;
    public Rect HitBoxRect { get { return new Rect(new Vector2(hitBox.bounds.center.x, hitBox.bounds.center.y), new Vector2(hitBox.bounds.extents.x, hitBox.bounds.extents.y)); } }
    protected float hitFlashTimer;
    public Material defaultMat, hitflashMat;
    protected float damageTimer = 0f;
    public bool CanTakeDamage { get { return (damageTimer <= 0f); } }

	void Start () {
        alive = true;
        hitBox = this.GetComponent<BoxCollider2D>();
        hitFlashTimer = 0f;
	}
	
	void Update () {
        if (hitFlashTimer > 0f)
        {
            hitFlashTimer -= Time.deltaTime;
            if (Time.frameCount % 8 > 4)
                this.GetComponent<MeshRenderer>().material = hitflashMat;
            else this.GetComponent<MeshRenderer>().material = defaultMat;
        }
        else this.GetComponent<MeshRenderer>().material = defaultMat;

        if (damageTimer > 0f)
            damageTimer -= Time.deltaTime;
	}

    public void TakeDamage(float damage)
    {
        hp -= damage;
        hitFlashTimer = 0.6f;
        damageTimer = 0.2f;
        if (hp < 0)
            alive = false;
    }
}