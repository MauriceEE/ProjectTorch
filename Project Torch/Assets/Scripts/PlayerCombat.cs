using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    //I'm just putting this here for ease of access, so "frame" data can be easily adjusted later
    protected float frame = 1f / 60f;

    protected enum CombatStates
    {
        None,
        Startup,
        Active,
        Recovery,
    }
    protected CombatStates combatState;

    protected enum Attacks
    {
        None,
        Slash,
        Thrust,
        Shine
    }
    protected Attacks currentAttack;

    //Whether or not the player is currently attacking
//    protected bool attacking = false;
    //Time spent since attack began
    protected float attackTime = 0f;
    //Whether or not the player can attack, can be accessed and modified by other scripts
    private bool canAttack = true;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    //Whether or not the player is holding the torch
    protected bool holdingTorch = true;

    [Header("Slash frame data")]
    public float slStartup = 6;
    public float slActive = 10;
    public float slRecovery = 8;
    public bool slEditorHitboxes = true;//DEBUG//
    public Rect slHB1;
    public float slHB2FirstActiveFrame = 4;
    public Rect slHB2;
    public float slHB3FirstActiveFrame = 7;
    public Rect slHB3;
    [Space(10)]
    [Header("Thrust frame data")]
    public float thStartup = 12;
    public float thActive = 7;
    public float thRecovery = 10;
    public bool thEditorHitboxes = true;//DEBUG//
    public Rect thHB1;
    public float thHB2FirstActiveFrame = 3;
    public Rect thHB2;
    public float thHB3FirstActiveFrame = 5;
    public Rect thHB3;
    [Header("Debug")]
    public Material normalMaterial;
    public Material attackingMaterial;
    public GameObject[] tempHitboxObj;

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.magenta;
        if (slEditorHitboxes)
        {
            Gizmos.color = Color.red;
            DrawHitbox(slHB1);
            Gizmos.color = Color.green;
            DrawHitbox(slHB2);
            Gizmos.color = Color.blue;
            DrawHitbox(slHB3);
        }
        if (thEditorHitboxes)
        {
            Gizmos.color = Color.red;
            DrawHitbox(thHB1);
            Gizmos.color = Color.green;
            DrawHitbox(thHB2);
            Gizmos.color = Color.blue;
            DrawHitbox(thHB3);
        }
    }

    void DrawHitbox(Rect hb)
    {
        //Top
        Debug.DrawLine(
            new Vector3(this.transform.position.x + hb.x, this.transform.position.y + hb.y, this.transform.position.z),
            new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y, this.transform.position.z));
        //Left
        Debug.DrawLine(
            new Vector3(this.transform.position.x + hb.x, this.transform.position.y + hb.y, this.transform.position.z),
            new Vector3(this.transform.position.x + hb.x, this.transform.position.y + hb.y - hb.height, this.transform.position.z));
        //Bottom
        Debug.DrawLine(
            new Vector3(this.transform.position.x + hb.x, this.transform.position.y + hb.y - hb.height, this.transform.position.z),
            new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y - hb.height, this.transform.position.z));
        //Right
        Debug.DrawLine(
            new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y, this.transform.position.z),
            new Vector3(this.transform.position.x + hb.x + hb.width, this.transform.position.y + hb.y - hb.height, this.transform.position.z));
    }

    void Start () {
        combatState = CombatStates.None;
        currentAttack = Attacks.None;
	}
	
	void Update () {
        if (canAttack)
            this.GetComponent<MeshRenderer>().material = normalMaterial;
        else
            this.GetComponent<MeshRenderer>().material = attackingMaterial;

        attackTime += Time.deltaTime;
        //Check to see if the player can attack
        if (combatState == CombatStates.None && canAttack)
        {
            //See if they input an attack button
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.J))//SQUARE / X
            {
                attackTime = 0f;
                //Start attacking
                combatState = CombatStates.Startup;
                currentAttack = Attacks.Slash;
                canAttack = false;
                this.GetComponent<PlayerMovement>().CanMove = false;
            }
            else if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.K))//TRIANGLE / Y
            {
                attackTime = 0f;
                //Start attacking
                combatState = CombatStates.Startup;
                currentAttack = Attacks.Thrust;
                canAttack = false;
                this.GetComponent<PlayerMovement>().CanMove = false;
            }
        }
        //DON'T make this an else or the player won't actually attack until one frame later
        if (combatState == CombatStates.Startup)
        {
            switch (currentAttack)
            {
                //Check to move to active frames
                case Attacks.Slash:
                    if (attackTime > slStartup * frame)
                        combatState = CombatStates.Active;
                    break;
                case Attacks.Thrust:
                    if (attackTime > thStartup * frame)
                        combatState = CombatStates.Active;
                    break;
                case Attacks.Shine:
                    break;
            }
        }
        if (combatState == CombatStates.Active) 
        {
            //Check to see if we need to activate another hitbox
            switch (currentAttack)
            {
                case Attacks.Slash:
                    // --do AABB for box 1--
                    GameObject tempObjBox1 = Instantiate(tempHitboxObj[0] as GameObject, this.transform);
                    tempObjBox1.transform.localPosition = new Vector3(slHB1.center.x, slHB1.center.y, 0);
                    if (attackTime > (slStartup + slHB2FirstActiveFrame) * frame)
                    {
                        // --do AABB for box 2--
                        GameObject tempObjBox2 = Instantiate(tempHitboxObj[1] as GameObject, this.transform);
                        tempObjBox2.transform.localPosition = new Vector3(slHB2.center.x, slHB2.center.y, 0);
                    }
                    if (attackTime > (slStartup + slHB3FirstActiveFrame) * frame)
                    {
                        // --do AABB for box 3--
                        GameObject tempObjBox3 = Instantiate(tempHitboxObj[2] as GameObject, this.transform);
                        tempObjBox3.transform.localPosition = new Vector3(slHB3.center.x, slHB3.center.y, 0);
                    }
                    if (attackTime > (slStartup + slActive) * frame)
                        combatState = CombatStates.Recovery;
                    break;
                case Attacks.Thrust:
                    // --do AABB for box 1--
                    GameObject tempObjBox1th = Instantiate(tempHitboxObj[0] as GameObject, this.transform);
                    tempObjBox1th.transform.localPosition = new Vector3(thHB1.center.x, thHB1.center.y, 0);
                    if (attackTime > (thStartup + thHB2FirstActiveFrame) * frame)
                    {
                        // --do AABB for box 2--
                        GameObject tempObjBox2th = Instantiate(tempHitboxObj[1] as GameObject, this.transform);
                        tempObjBox2th.transform.localPosition = new Vector3(thHB2.center.x, thHB2.center.y, 0);
                    }
                    if (attackTime > (thStartup + thHB3FirstActiveFrame) * frame)
                    {
                        // --do AABB for box 3--
                        GameObject tempObjBox3th = Instantiate(tempHitboxObj[2] as GameObject, this.transform);
                        tempObjBox3th.transform.localPosition = new Vector3(thHB3.center.x, thHB3.center.y, 0);
                    }
                    if (attackTime > (thStartup + thActive) * frame)
                        combatState = CombatStates.Recovery;
                    break;
                case Attacks.Shine:
                    break;
            }
        }
        if (combatState == CombatStates.Recovery)
        {
            switch (currentAttack)
            {
                case Attacks.Slash:
                    if (attackTime > (slStartup + slActive + slRecovery) * frame)
                    {
                        combatState = CombatStates.None;
                        canAttack = true;
                        this.GetComponent<PlayerMovement>().CanMove = true;
                        attackTime = 0f;
                    }
                    break;
                case Attacks.Thrust:
                    if (attackTime > (thStartup + thActive + thRecovery) * frame)
                    {
                        combatState = CombatStates.None;
                        canAttack = true;
                        this.GetComponent<PlayerMovement>().CanMove = true;
                        attackTime = 0f;
                    }
                    break;
                case Attacks.Shine:
                    break;
            }
        }
	}
}