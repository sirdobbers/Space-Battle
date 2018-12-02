using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Modular thruster script -> prefab
 *      - Add a thruster Prefab to a ship Object
 *      - Place it where it should stay
 *      - Set its activation parameters (turnleft, runt right, etc.)
 * 
 */

public class Thurster : MonoBehaviour {
    public enum ThrustDirection
    {
        turnLeft, turnRight, forward, backward, strafeLeft, strafeRight, boost
    }
    public ThrustDirection Dir;

    GameObject Parent;
    Vector3 initScale;
    float ThrustLvl = 0;
    
    void Start () {
        initScale = transform.localScale;
        Parent = transform.parent.gameObject;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = Parent.GetComponent<SpriteRenderer>().sortingLayerID;
    }
	
	void Update () {

        if (Dir == ThrustDirection.forward) { ThrustLvl = transform.parent.GetComponent<Ship>().GetThrust("forward"); }
        else if (Dir == ThrustDirection.backward) { ThrustLvl = -transform.parent.GetComponent<Ship>().GetThrust("forward"); }
        else if (Dir == ThrustDirection.turnRight) { ThrustLvl = transform.parent.GetComponent<Ship>().GetThrust("rotate"); }
        else if (Dir == ThrustDirection.turnLeft) { ThrustLvl = -transform.parent.GetComponent<Ship>().GetThrust("rotate"); }
        else if (Dir == ThrustDirection.strafeLeft) { ThrustLvl = transform.parent.GetComponent<Ship>().GetThrust("strafe"); }
        else if (Dir == ThrustDirection.strafeRight) { ThrustLvl = -transform.parent.GetComponent<Ship>().GetThrust("strafe"); }
        else if (Dir == ThrustDirection.boost) { ThrustLvl = Mathf.Clamp(transform.parent.GetComponent<Ship>().GetThrust("boost"), 0, 1); }

        //this is the "pulsing" effect of the thrusters
        transform.localScale = initScale * (ThrustLvl) * Random.Range(1.0f- 0.2f * initScale.magnitude, 1.0f + 0.2f * initScale.magnitude);

    }
}
