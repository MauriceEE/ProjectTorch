using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float minY, maxY;//NOTE FOR ME: make visuals for these later so they can be set up easily during level design
    public Vector2 moveSpeed;

    //Speed this current frame - add to this before calling Move
    protected Vector2 displacement;

	void Start () {
		
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

        //Move the player
        this.Move();
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
}
