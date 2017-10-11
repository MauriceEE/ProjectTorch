using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
    //store background objects
    public GameObject Ground;
    public GameObject Sky;
    public GameObject Background;
    public GameObject Clouds;
    public GameObject Front;
    public GameObject Details;



    //speed multipliers (1.0 is the current player speed)

    //this affects ALL background elements at once
    public float globalSpeedMultiplier;

    public float groundSpeedMultiplier;
    public float skySpeedMultiplier;
    public float frontSpeedMultiplier;
    public float cloudSpeedMultiplier;
    public float backgroundSpeedMultiplier;

    public Vector3 battlefieldSpawn;
    public Vector3 sullenVillageSpawn;
    public Vector3 thrivingVillageSpawn;
    
    /*float groundSpeed;
    float skySpeed;
    float frontSpeed;
    float cloudSpeed;
    */
    public GameObject Player;
    PlayerMovement pmover;
    float moveSpeed;
	// Use this for initialization
	void Start () {
        pmover = Player.GetComponent<PlayerMovement>();
        moveSpeed = pmover.GetSpeed().x * -120;
    }
	
	// Update is called once per frame
	void Update () {
        moveSpeed = pmover.GetSpeed().x * -120;
        
        Ground.transform.position +=  new Vector3(Time.deltaTime * moveSpeed * groundSpeedMultiplier * globalSpeedMultiplier,0f,0f);
        Details.transform.position += new Vector3(Time.deltaTime * moveSpeed * groundSpeedMultiplier * globalSpeedMultiplier, 0f, 0f);

        Background.transform.position += new Vector3(Time.deltaTime * moveSpeed * backgroundSpeedMultiplier * globalSpeedMultiplier, 0f, 0f);
        Clouds.transform.position += new Vector3(Time.deltaTime * moveSpeed * cloudSpeedMultiplier * globalSpeedMultiplier, 0f, 0f);
        Sky.transform.position += new Vector3(Time.deltaTime * moveSpeed * skySpeedMultiplier * globalSpeedMultiplier, 0f, 0f);
        Front.transform.position += new Vector3(Time.deltaTime * moveSpeed * frontSpeedMultiplier * globalSpeedMultiplier, 0f, 0f);
    }
    public void MoveByBackgroundOffset(Transform entity) {
        entity.position += new Vector3(Time.deltaTime * pmover.GetSpeed().x * -120 * globalSpeedMultiplier, 0f, 0f);
    }
}
