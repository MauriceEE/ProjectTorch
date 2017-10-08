using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages complex player movement and controller input
/// Communicates with PlayerCombat to make sure they don't move while attacking and whatnot
/// </summary>
public class PlayerMovement : MonoBehaviour {
    #region Public Fields
    public Vector2 moveSpeed;
    [Header("Dash data")]
    public float dashIFrames = 6;
    public float dashFrames = 10;
    public Vector2 dashSpeed;
    //Makes the dash more smooth
    [Range(0.50f, 1.00f)]
    public float dashFriction;
    #endregion
    #region Private Fields
    //Speed the player inputs
    protected Vector2 inputDisplacement;
    //Obstalces to check for during collision //NOT doing collision anymore
    //protected Collider2D[] obstacles;
    //A reference to the player's bounds just to make things less tedious to write
    protected Collider2D playerCollider;
    //Whether or not the player can dodge, can be accessed and modified by other scripts
    protected bool canDash = true;
    //Time left for the player to dash
    public float dashTime = 0f;
    //Reference to the combat class
    protected PlayerCombat combat;
    //Whether the player can take damage
    protected bool invincible = false;
    //Reference to Entity base script
    protected Entity entity;
    #endregion
    #region Properties
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    #endregion
    #region Unity Methods
    void Start () {
        canDash = true;
        combat = this.GetComponent<PlayerCombat>();
        playerCollider = this.GetComponent<Collider2D>();
        entity = this.GetComponent<Entity>();
        entity.Speed = moveSpeed;
        //GameObject[] obstacleGameObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        //obstacles = new Collider2D[obstacleGameObjects.Length];
        //for (int i = 0; i < obstacles.Length; ++i)
            //obstacles[i] = obstacleGameObjects[i].GetComponent<Collider2D>();
	}
	
	void Update () {
        
        //If NOT DASHING
        if (dashTime <= 0f)
        {
            //Reset displacement this frame if not dashing
            entity.Displacement = Vector2.zero;

            //Move with controller
            inputDisplacement = new Vector2(Input.GetAxis("Horizontal") * entity.Speed.x, Input.GetAxis("Vertical") * entity.Speed.y);

            //Check to see if they want to dash
            if (canDash && (Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.L))) 
            {
                //Start the dash timer
                dashTime = dashFrames * Helper.frame;

                //Set initial velocity of dash
                inputDisplacement.Normalize();
                entity.Displacement = new Vector2(inputDisplacement.x * dashSpeed.x, inputDisplacement.y * dashSpeed.y);

                //Can't move or attack while dashing, gotta commit
                entity.CanMove = false;
                combat.CanAttack = false;
            }
        }
        //If DASHING
        if (dashTime > 0f)
        {
            //Check to see if you're still invincible
            if (dashTime > (dashFrames - dashIFrames) * Helper.frame)
                invincible = true;
            else
                invincible = false;

            //Scale down displacement with dampening
            entity.Displacement *= dashFriction;

            //Check for collisions
            //this.CheckCollisions();

            //Move the player
            entity.Move();

            //Reduce dash time remaining
            dashTime -= Time.deltaTime;

            //Check if you can now move and attack
            if(dashTime<=0f)
            {
                entity.CanMove = true;
                combat.CanAttack = true;
                invincible = false; // sets to false here to ensure that this is turned off in case the player cancels the dash during the invincibility period
                return;//don't fall through into the normal movement stuff
            }
        }
        //Normal movement when not dashing
        if (entity.CanMove && dashTime <= 0f) 
        {
            //Apply displacement
            entity.Displacement = inputDisplacement;

            //Update direction
            if (entity.Displacement.x > 0.001f)
                entity.FacingRight = true;
            else if (entity.Displacement.x < -0.001f)
                entity.FacingRight = false;

            //Make sure you won't run into an obstacle
            //this.CheckCollisions();

            //Move the player
            entity.Move();
        }
    }
    //get the speed of the player, necessary for background scrolling
    public Vector3 GetSpeed() {
        if(entity != null) {
            return entity.Displacement;
        }else {
            return new Vector3();
        }

    }
#endregion
}
