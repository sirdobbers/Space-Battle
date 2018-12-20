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

    Ship_Movement moveComp;


    void Start () {
        initScale = transform.localScale;
        Parent = transform.parent.gameObject;


        moveComp = Parent.GetComponent<Ship_Movement>();
        if (moveComp == null) { Debug.LogError("Thruster requires Ship_Movement Component"); }

        //gameObject.GetComponent<SpriteRenderer>().sortingLayerID = Parent.GetComponent<SpriteRenderer>().sortingLayerID;
    }
	
	void Update () {

        if (moveComp != null) {
            if (Dir == ThrustDirection.forward) { ThrustLvl = moveComp.forward; }
            else if (Dir == ThrustDirection.backward) { ThrustLvl = -moveComp.forward; }
            else if (Dir == ThrustDirection.turnRight) { ThrustLvl = moveComp.rotate; }
            else if (Dir == ThrustDirection.turnLeft) { ThrustLvl = -moveComp.rotate; }
            else if (Dir == ThrustDirection.strafeLeft) { ThrustLvl = moveComp.strafe; }
            else if (Dir == ThrustDirection.strafeRight) { ThrustLvl = -moveComp.strafe; }
            else if (Dir == ThrustDirection.boost) { ThrustLvl = moveComp.boost; }
        }

        //this is the "pulsing" effect of the thrusters
        transform.localScale = initScale * (ThrustLvl) * Random.Range(1.0f- 0.1f * initScale.magnitude, 1.0f + 0.1f * initScale.magnitude);

    }
}
