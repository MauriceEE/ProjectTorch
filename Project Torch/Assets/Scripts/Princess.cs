using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour {

    #region Enums
    public enum PrincessStates
    {
        Escorted,
        CoweringInFear,
        Fleeing
    }
    #endregion
    #region Fields
    protected Entity entity;
    protected PrincessStates state;
    protected Vector2 moveTarget;
    public Enemy_PrincessEscort escortee;
    public float maxVelocity;
    public float arrivalRadius;
    public float escortedSpeedModifier;
    public float fleeingSpeedModifier;
    #endregion
    #region Properties
    public PrincessStates State { get { return state; } set { state = value; } }
    #endregion

    protected void Awake() {
        entity = this.GetComponent<Entity>();
        state = PrincessStates.Escorted;
        moveTarget = Vector2.zero;
	}
	
	protected void Update () {
        switch (state)
        {
            case PrincessStates.Escorted:
                entity.SpeedModifier = escortedSpeedModifier;
                moveTarget = escortee.transform.position;
                SeekTarget();
                break;
            case PrincessStates.CoweringInFear:
                entity.SpeedModifier = 0f;
                moveTarget = this.transform.position;
                //TODO: A shake/shiver thing
                break;
            case PrincessStates.Fleeing:
                entity.SpeedModifier = fleeingSpeedModifier;
                moveTarget = new Vector2(100000f, this.transform.position.y);
                SeekTarget();
                break;
        }
        entity.Move();
	}

    /// <summary>
    /// Seeks out the 'moveTarget' vector location
    /// </summary>
    protected void SeekTarget()
    {
        //Get deisred velocity
        Vector2 desiredDisplacement = moveTarget - new Vector2(this.transform.position.x, this.transform.position.y);
        //Apply displacement
        this.entity.Displacement += desiredDisplacement;
        //Limit based on max speed
        this.entity.Displacement = Vector2.ClampMagnitude(this.entity.Displacement, maxVelocity);
        //See if you're close enough to slow down
        float dist = desiredDisplacement.magnitude;
        if (dist <= arrivalRadius)
            this.entity.Displacement *= dist / arrivalRadius;
    }

    public void SavePrincess()
    {
        //Trigger dialogue
        GameObject.Find("FlagManagerGO").GetComponent<FlagManager>().ActivateDialogueLines("Princess - Saved");
        //
        state = PrincessStates.Fleeing;
    }
}