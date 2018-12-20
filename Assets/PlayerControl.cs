using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Ship shipComp;
    private Ship_Movement moveComp;

    void Start() {
        shipComp = GetComponent<Ship>();
        if (shipComp == null) { Debug.LogError("PlayerControl requires Ship Component"); }
        moveComp = GetComponent<Ship_Movement>();
        if (moveComp == null) { Debug.LogError("PlayerControl requires Ship_Movement Component"); }
    }
    
    void Update() {
        Move();
        HandleTurrets();
    }
    
    public void Move() {
        float forward = Input.GetAxis("Throttle");
        float rotate = Input.GetAxis("Turn");
        float strafe = Input.GetAxis("Strafe");
        float boost = Input.GetAxis("Boost");

        if (shipComp.control == Ship.Control.Player) {
            moveComp.Translate(forward, strafe, boost);
            moveComp.Rotate(rotate);
        }
        else if (shipComp.control == Ship.Control.PlayerAlt) {
            Vector3 moveDir = new Vector3(rotate,forward);
            bool active = false;
            if (forward != 0 || rotate != 0) {
                moveComp.RotateToDirection(moveDir);
                active = true;
            }
            moveComp.WorldTranslate(moveDir,active,boost,strafe);
        }
    }

    public void HandleTurrets() {
        // FIRE FIXED GUNS
        if (Input.GetMouseButton(0)) {
            for (int i = 0; i < shipComp.FixedGunArray.Count; i++) {
                shipComp.FixedGunArray[i].GetComponent<FixedGun>().Fire();
            }
        }
        // FIRE TURRETS
        if (Input.GetMouseButton(1)) {
            for (int i = 0; i < shipComp.TurretArray.Count; i++) {
                shipComp.TurretArray[i].GetComponent<Turret>().Fire();
            }
        }
        // TURN TURRETS
        for (int i = 0; i < shipComp.TurretArray.Count; i++) {
            shipComp.TurretArray[i].GetComponent<Turret>().RotateTurretToMouseLocation();
        }
    }
}
