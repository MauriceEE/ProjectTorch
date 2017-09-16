using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    protected float minY, maxY;//NOTE FOR ME: make visuals for these later so they can be set up easily during level design
    public Vector2 moveSpeed;

    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement;
    //Obstalces to check for during collision
    protected Collider2D[] obstacles;
    //A reference to the player's bounds just to make things less tedious to write
    protected Collider2D playerCollider;

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
        //Reset displacement this frame
        displacement = Vector2.zero;

        // --TEMP-- //
        //Move with keyboard keys
        if (Input.GetKey(KeyCode.A))
            displacement += new Vector2(-moveSpeed.x, 0);
        else if (Input.GetKey(KeyCode.D))
            displacement += new Vector2(moveSpeed.x, 0);
        if (Input.GetKey(KeyCode.W))
            displacement += new Vector2(0, moveSpeed.y);
        else if (Input.GetKey(KeyCode.S))
            displacement += new Vector2(0, -moveSpeed.y);

        //Make sure you won't run into an obstacle
        this.CheckCollisions();
        //Move the player
        this.Move();
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
            //See if we're inside the obstacle
            if (AABB(obstacles[i]))
            {
                displacement = Vector2.zero;
            }
        }
    }

    protected bool AABB(Collider2D other)
    {
        Rect r1 = new Rect(new Vector2(playerCollider.bounds.center.x + displacement.x, playerCollider.bounds.center.y + displacement.y), new Vector2(playerCollider.bounds.extents.x, playerCollider.bounds.extents.y));
        Rect r2 = new Rect(new Vector2(other.bounds.center.x, other.bounds.center.y), new Vector2(other.bounds.extents.x, other.bounds.extents.y));
        return (r1.x < r2.x + r2.width && r1.x + r1.width > r2.x && r1.y < r2.y + r2.height && r1.height + r1.y > r2.y) ;
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
