using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Base level entity class, should be placed on all moving objects
/// Stuff to know:
///     Properties:
///         Vector2 Speed - max speed of the entity, multiply by a direction vector to get displacement
///         Vector2 Displacement - the amount of Unity units the entity will move when Move() is called
///         bool CanMove - whether or not the entity can move; NOT tested in Move()
///         bool FacingRight - whether or not the entity is facing right, for attacking and sprites
///     Methods:
///         void Move() - translates transform by Displacement and then makes sure it's still in bounds of the level
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour {
    #region Private Fields
    //Maximum and minimum world positions for the entity
    protected float minY, maxY;//TODO: make visuals for these later so they can be set up easily during level design
    //Movement speed of the entity
    protected Vector2 speed;
    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement = Vector2.zero;
    //Whether or not the entity can move
    protected bool canMove = true;
    //Whether or not the entity is facing right
    protected bool right = true;
    //Reference to the hitbox
    protected BoxCollider2D hitBox;
    //Scale to apply to speed, use with slows and whatnot
    protected float speedModifier;
    #endregion

    #region Public Fields
    //ID which will be cross-referenced in other managers to determine dialogue
    public TextManager.InteractiveNPCNames npcID;
    #endregion

    #region Properties
    public Vector2 Speed { get { return speed; } set { speed = value; } }
    public Vector2 Displacement { get { return displacement; } set { displacement = value; } }
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    public bool FacingRight { get { return right; } set { right = value; } }
    public float SpeedModifier { get { return speedModifier; } set { speedModifier = value; } }
    public Rect HitBoxRect { get { return new Rect(new Vector2(hitBox.bounds.center.x, hitBox.bounds.center.y), new Vector2(hitBox.bounds.extents.x, hitBox.bounds.extents.y)); } }
    #endregion

    #region Unity Methods
    void Start()
    {
        minY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MinY;
        maxY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MaxY;
        hitBox = this.GetComponent<BoxCollider2D>();
        speedModifier = 1f;//Always start with default speed
    }
    #endregion

    #region Custom Methods
    public void Move()
    {
        //Attempt to move
        this.transform.position += Helper.Vec2ToVec3(displacement * speedModifier);
        //Check to see if we've changed direction
        if (displacement.x != 0) 
        {
            //Update right
            right = (displacement.x > 0);
            //Update sprite if flipping is necessary
            this.GetComponent<SpriteRenderer>().flipX = right;
        }
        //Keep within bounds of level
        if (this.transform.position.y > maxY)
            this.transform.position = new Vector3(this.transform.position.x, maxY, this.transform.position.z);
        else if (this.transform.position.y < minY)
            this.transform.position = new Vector3(this.transform.position.x, minY, this.transform.position.z);
    }

    /// <summary>
    /// Modifies displacement if you'd otherwise move inside an object tagged as an Obstacle
    /// Should be called before Move()
    /// </summary>
    /*Not using collisions anymore?
    protected void CheckCollisions()
    {
        //Loop through obstacles in the level
        for (int i = 0; i < obstacles.Length; ++i)
        {
            Rect r1 = new Rect(new Vector2(playerCollider.bounds.center.x + displacement.x, playerCollider.bounds.center.y + displacement.y), new Vector2(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y));
            Rect r2 = new Rect(new Vector2(obstacles[i].bounds.center.x, obstacles[i].bounds.center.y), new Vector2(obstacles[i].bounds.extents.x, obstacles[i].bounds.extents.y));
            //See if we're inside the obstacle
            if (Helper.AABB(r1, r2))
            {
                displacement = Vector2.zero;
            }
        }
    }*/
#endregion
}
