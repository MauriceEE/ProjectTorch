using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    protected float frame = 1f / 60f;

    protected float minY, maxY;//NOTE FOR ME: make visuals for these later so they can be set up easily during level design
    public Vector2 moveSpeed;
    [Header("Dash data")]
    public float dashIFrames = 6;
    public float dashFrames = 10;
    public Vector2 dashSpeed;
    //Makes the dash more smooth
    [Range(0.50f, 1.00f)]
    public float dashFriction;

    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement;
    //Speed the player inputs
    protected Vector2 inputDisplacement;
    //Obstalces to check for during collision
    protected Collider2D[] obstacles;
    //A reference to the player's bounds just to make things less tedious to write
    protected Collider2D playerCollider;
    //Whether or not the player can move, can be accessed and modified by other scripts
    protected bool canMove = true;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    //Whether or not the player can dodge, can be accessed and modified by other scripts
    protected bool canDash = true;
    public bool CanDash { get { return canDash; } set { canDash = value; } }
    //Whether or not the player is facing right
    protected bool right = true;
    public bool FacingRight { get { return right; }  }
    //Time left for the player to dash
    protected float dashTime = 0f;
    //Reference to the combat class
    protected PlayerCombat combat;
    //Whether the player can take damage
    protected bool invincible = false;

    void Start () {
        canDash = true;
        minY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MinY;
        maxY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MaxY;
        combat = this.GetComponent<PlayerCombat>();
        playerCollider = this.GetComponent<Collider2D>();
        GameObject[] obstacleGameObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacles = new Collider2D[obstacleGameObjects.Length];
        for (int i = 0; i < obstacles.Length; ++i)
            obstacles[i] = obstacleGameObjects[i].GetComponent<Collider2D>();
	}
	
	void Update () {
        
        //If NOT DASHING
        if (dashTime <= 0f)
        {
            //Reset displacement this frame if not dashing
            displacement = Vector2.zero;

            //Move with controller
            inputDisplacement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed.x, Input.GetAxis("Vertical") * moveSpeed.y);

            //Check to see if they want to dash
            if (canDash && Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.L))
            {
                //Start the dash timer
                dashTime = dashFrames * frame;

                //Set initial velocity of dash
                inputDisplacement.Normalize();
                displacement = new Vector2(inputDisplacement.x * dashSpeed.x, inputDisplacement.y * dashSpeed.y);

                //Can't move or attack while dashing, gotta commit
                canMove = false;
                combat.CanAttack = false;
            }
        }
        //If DASHING
        if (dashTime > 0f)
        {
            //Check to see if you're still invincible
            if (dashTime > (dashFrames - dashIFrames) * frame)
                invincible = true;
            else
                invincible = false;

            //Scale down displacement with dampening
            displacement *= dashFriction;

            //Check for collisions
            this.CheckCollisions();

            //Move the player
            this.Move();

            //Reduce dash time remaining
            dashTime -= Time.deltaTime;

            //Check if you can now move and attack
            if(dashTime<0f)
            {
                canMove = true;
                combat.CanAttack = true;
                return;//don't fall through into the normal movement stuff
            }
        }
        //Normal movement when not dashing
        if (canMove && dashTime <= 0f) 
        {
            //Apply displacement
            displacement = inputDisplacement;

            //Update direction
            if (displacement.x > 0.001f)
                right = true;
            else if (displacement.x < -0.001f)
                right = false;

            //Make sure you won't run into an obstacle
            this.CheckCollisions();

            //Move the player
            this.Move();
        }
    }

    /// <summary>
    /// Modifies displacement if you'd otherwise move inside an object tagged as an Obstacle
    /// Should be called before Move()
    /// </summary>
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
    }

    protected void Move()
    {
        //Attempt to move
        this.transform.position += new Vector3(displacement.x, displacement.y, 0);
        //Keep within bounds of level
        if (this.transform.position.y > maxY)
            this.transform.position = new Vector3(this.transform.position.x, maxY, this.transform.position.z);
        else if (this.transform.position.y < minY)
            this.transform.position = new Vector3(this.transform.position.x, minY, this.transform.position.z);
    }

    /*Don't think we'll use this
    protected void DashMove()
    {
        //Attempt to move
        this.transform.position += new Vector3(displacement.x, displacement.y, 0);
        //Keep within bounds of level
        if (this.transform.position.y > maxY)
            this.transform.position = new Vector3(this.transform.position.x, maxY, this.transform.position.z);
        else if (this.transform.position.y < minY)
            this.transform.position = new Vector3(this.transform.position.x, minY, this.transform.position.z);
    }*/

    /*Don't think we'll use this
    protected class BoxCollisionHitInfo
    {
        protected bool[] corners;//0 = top left, 1 = top right, 2 = low left, 3 = low right

        public BoxCollisionHitInfo(bool topLeft, bool topRight, bool lowLeft, bool lowRight)
        {
            corners = new bool[4];
            corners[0] = topLeft;
            corners[1] = topRight;
            corners[2] = lowLeft;
            corners[3] = lowRight;
        }

        public BoxCollisionHitInfo()
        {
            corners = new bool[4];
            corners[0] = false;
            corners[1] = false;
            corners[2] = false;
            corners[3] = false;
        }

        public bool TopLeft
        { get { return corners[0]; } }
        public bool TopRight
        { get { return corners[1]; } }
        public bool LowLeft
        { get { return corners[2]; } }
        public bool LowRight
        { get { return corners[3]; } }
    }*/
}
