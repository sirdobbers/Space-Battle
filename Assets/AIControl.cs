using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
    Ship shipComp;
    Ship_Movement moveComp;
    ShipScanner scanComp;

    float maxRange = 4;

    GameObject target;
    Vector3 myDir;
    float myAng;
    Vector3 targPos;
    float targDist;
    Vector3 targDir;
    float targAng;
    float angDiff;

    void Start() {
        shipComp = GetComponent<Ship>();
        if (shipComp == null) { Debug.LogError("AIControl requires Ship Component"); }
        moveComp = GetComponent<Ship_Movement>();
        if (moveComp == null) { Debug.LogError("AIControl requires Ship_Movement Component"); }
        scanComp = GetComponent<ShipScanner>();
        if (scanComp == null) { Debug.LogError("AIControl requires ShipScanner Component"); }

    }

    void Update() {
        scanComp.Scan();
        target = scanComp.GetClosestShip();
        myDir = transform.up; myDir.Normalize();
        myAng = Mathf.Atan2(myDir.y, myDir.x) * Mathf.Rad2Deg;
        if (target != null) {
            targPos = target.transform.position;
            targDist = Vector3.Distance(targPos, transform.position);
            targDir = targPos - transform.position; targDir.Normalize();
            targAng = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;
        }
        angDiff = Mathf.DeltaAngle(targAng, myAng);

        Move();
        HandleTurrets();
    }

    public void Move() {
        if (shipComp.control == Ship.Control.BasicAI) {
            if (target != null) {
                moveComp.RotateToTarget(target);
            }
            moveComp.Translate(1 * Random.Range(0.5f, 1.5f), 0, 0);
        }
        else if (shipComp.control == Ship.Control.AdvancedAI) {
            if (target != null) {
                if (targDist > maxRange) {
                    moveComp.RotateToTarget(target);
                }
                else if (targDist < (maxRange / 4f)) {
                    Vector3 newDir = Vector3.Cross(targDir, new Vector3(0, 0, 1));
                    moveComp.RotateToPosition(transform.position + newDir);
                }
                else {
                    Vector3 targVel = target.GetComponent<Ship_Movement>().GetVel();
                    Vector3 relVel = targVel - moveComp.GetVel();
                    Vector3 newDir = relVel;
                    moveComp.RotateToPosition(transform.position + newDir);
                }
            }
            moveComp.Translate(1 * Random.Range(0.5f, 1.5f), 0, 0);
        }
    }

    public void HandleTurrets() {
        // FIRE GUNS
        if (shipComp.control != Ship.Control.None) {
            if (Mathf.Abs(angDiff) < 5) {
                for (int i = 0; i < shipComp.FixedGunArray.Count; i++) {
                    shipComp.FixedGunArray[i].GetComponent<FixedGun>().Fire();
                }
            }
            // ROTATE & FIRE TURRETS
            for (int i = 0; i < shipComp.TurretArray.Count; i++) {
                if (target != null) {
                    Turret T = shipComp.TurretArray[i].GetComponent<Turret>();
                    T.SetTarget(target);
                    T.RotateTurretToTarget(true);
                    T.ValidFire();
                }
            }
        }
    }
}
