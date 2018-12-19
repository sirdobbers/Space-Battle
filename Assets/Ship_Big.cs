using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Big : Ship {

    void Start () {
        base.CStart();
    }
	
	// Update is called once per frame
	void Update () {
        base.CUpdate();
        base.AdvancedAIMovementControl();
        base.GenericAITurretControl();
    }
}
