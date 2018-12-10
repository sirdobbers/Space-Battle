using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Modular Turret script : Component of prefab Turret
 *      - Place Turret on object
 *      - Set its type and properties through its child prefab "fixedGun"
 * 
 */

public class Turret : MonoBehaviour {
    public float rotSpeed = 90f;
    public float rotateRange = 360; // degrees
    public float maxRange = 50;
    public bool debug = false;

    private ShipScanner Scanner;
    private GameObject Parent;
    private GameObject Child_Gun;
    private GameObject Target;
    private Vector3 predictedAimPos;
    private float bulletSpeed;
    private float scanTimer;
    private float targDist;

    void Start() {
        Parent = transform.parent.gameObject;
        Child_Gun = transform.GetChild(0).gameObject;
        bulletSpeed = Child_Gun.GetComponent<FixedGun>().projectileInitSpeed;
        Scanner = gameObject.GetComponent<ShipScanner>();
    }

    void Update () {
        if (Target != null) {
            targDist = Vector3.Distance(Target.transform.position, transform.position);
        }
    }
    
    public void HandleAutoTurret() {
        if (Target == null) {
            Scanner.ScanInInterval(5f);
            Target = Scanner.ClosestShip;
        }
        if (targDist <= maxRange) {
            RotateTurretToTarget(true);
            FireIfValid();
        }
    }

    #region Rotation Methods
    public void RotateTurretToDir(Vector3 dir) {
        dir.Normalize();
        float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion desiredRot = Quaternion.Euler(0, 0, dirAngle - 90);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
    public void RotateTurretToMouseLocation() {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Child_Gun.transform.position;
        RotateTurretToDir(dir);
    }
    public void RotateTurretToPosition(Vector3 position) {
        Vector3 dir = position - Child_Gun.transform.position;
        RotateTurretToDir(dir);
    }
    public void RotateTurretToTarget(bool prediction) { //set predition to true to use predicted value
        if (Target != null) {
            Vector3 dir;
            if (prediction) {
                predictedAimPos = transform.position + gameObject.GetComponent<BulletPrediction2>().GetAimLocation(Target, gameObject, bulletSpeed * Time.deltaTime);
                dir = predictedAimPos - Child_Gun.transform.position;
            }
            else {dir = Target.transform.position - Child_Gun.transform.position;}
            RotateTurretToDir(dir);
        }
    }
    #endregion

    //CHECKS TO SEE IF THE PREDICTED TARGET POSITION IS WITHIN A BEARING
    public bool TargetWithinAng(float a) {

        Vector3 predictedAimPos = transform.position + gameObject.GetComponent<BulletPrediction2>().GetAimLocation(Target, gameObject, bulletSpeed * Time.deltaTime);
        Vector3 predTargDir = predictedAimPos - transform.position; predTargDir.Normalize(); //get pred target dir rel to gun
        float predTargAng = Mathf.Atan2(predTargDir.y, predTargDir.x) * Mathf.Rad2Deg; //get that ang

        float turretAng = GetGunAng();
        float angDiff = Mathf.DeltaAngle(turretAng, predTargAng);
        if (Mathf.Abs(angDiff) < a) {
            return true;
        }
        return false;
    }

    public void FireIfValid() {
        if (Target != null) {
            if (targDist <= maxRange && TargetWithinAng(10)) {
                Child_Gun.GetComponent<FixedGun>().Fire();
            }
        }
    }
    public void Fire() {
        Child_Gun.GetComponent<FixedGun>().Fire();
    }

    #region Getters Setters and Debug
    public float GetGunAng() {
        Vector3 myGunDir = Child_Gun.transform.up;
        return Mathf.Atan2(myGunDir.y, myGunDir.x) * Mathf.Rad2Deg;
    }

    public void SetTarget(GameObject T) {
        Target = T;
    }

    void OnDrawGizmos() {
        if (debug) { 
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, maxRange);
            //Gizmos.DrawSphere(predictedAimPos, 0.5f);
        }
    }
    #endregion
}
