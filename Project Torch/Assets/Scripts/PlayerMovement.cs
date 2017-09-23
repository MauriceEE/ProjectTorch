using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    protected float minY, maxY;//NOTE FOR ME: make visuals for these later so they can be set up easily during level design
    public Vector2 moveSpeed;
    public Vector2 dashSpeed;
    //Max time the player can dash
    public float maxDashTime;

    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement;
    //Obstalces to check for during collision
    protected Collider2D[] obstacles;
    //A reference to the player's bounds just to make things less tedious to write
    protected Collider2D playerCollider;
    //Whether or not the player can move, can be accessed and modified by other scripts
    protected bool canMove = true;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    //Whether or not the player is facing right
    protected bool right = true;
    public bool FacingRight { get { return right; }  }
    //Time left for the player to dash
    protected float dashTime = 0f;
    

    void Start () {
        minY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MinY;
        maxY = GameObject.Find("ZAxisManagerGO").GetComponent<ZAxisManager>().MaxY;
        playerCollider = this.GetComponent<Collider2D>();
        GameObject[] obstacleGameObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacles = new Collider2D[obstacleGameObjects.Length];
        for (int i = 0; i < obstacles.Length; ++i)
            obstacles[i] = obstacleGameObjects[i].GetComponent<Collider2D>();
	}
	
	void Update () {

        if (canMove)
        {
            //Reset displacement this frame
            displacement = Vector2.zero;

            //Move with controller
            displacement = new Vector2(Input.GetAxis("Horizontal") * moveSpeed.x, Input.GetAxis("Vertical") * moveSpeed.y);

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
    }
}
