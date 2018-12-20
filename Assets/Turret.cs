using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Modular Turret script : Component of prefab Turret
 *      - Place Turret on object
 *      - Set its type through its child prefab "fixedGun" (gun, missle, laser)
 * 
 */

public class Turret : MonoBehaviour {
    public float rotSpeed = 90f;
    public float rotateRange = 360; // degrees
    public float targetRange = 20;
    public bool debug = false;

    private GameObject Parent;
    private GameObject Child_Gun;
    private GameObject Target;
    private Vector3 predictedAimPos;
    float distance;
    float bulletSpeed;



    void Start() {
        Parent = transform.parent.gameObject;
        Child_Gun = transform.GetChild(0).gameObject;

        //set physics layer and local render layer
        //gameObject.GetComponent<SpriteRenderer>().sortingLayerID = Parent.GetComponent<SpriteRenderer>().sortingLayerID;
        //gameObject.GetComponent<SpriteRenderer>().sortingOrder = Parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

        bulletSpeed = Child_Gun.GetComponent<FixedGun>().projectileInitSpeed;
    }

    void Update () {
        if (Target != null) {
            distance = Vector3.Distance(Target.transform.position, transform.position);
            predictedAimPos = transform.position + gameObject.GetComponent<BulletPrediction>().GetAimLocation(Target, gameObject, bulletSpeed * Time.deltaTime);
        }
    }
    
    public void RotateTurretToMouseLocation() {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Child_Gun.transform.position;
        float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion desiredRot = Quaternion.Euler(0, 0, dirAngle - 90);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime); 
    }

    public void RotateTurretToPosition(Vector3 position) {
        Vector3 dir = position - Child_Gun.transform.position;
        dir.Normalize();
        float dirAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion desiredRot = Quaternion.Euler(0, 0, dirAngle - 90);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        
    }

    public void RotateTurretToTarget(bool prediction) { //set predition to true to use predicted value

        if (distance <= targetRange & Target != null) {
            Vector3 dirToTarget;
            if (prediction) {dirToTarget = predictedAimPos - Child_Gun.transform.position;}
            else {dirToTarget = Target.transform.position - Child_Gun.transform.position;}
            dirToTarget.Normalize();
            float dirAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg;
            Quaternion desiredRot = Quaternion.Euler(0, 0, dirAngle - 90);
            Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        }
    }
    
    //CHECKS TO SEE IF THE PREDICTED TARGET POSITION IS WITHIN A BEARING
    public bool TargetWithinAng(float a) {
        float turretAng = GetGunAng(); //get gun ang

        Vector3 predTargDir = predictedAimPos - transform.position; predTargDir.Normalize(); //get pred target dir rel to gun
        float predTargAng = Mathf.Atan2(predTargDir.y, predTargDir.x) * Mathf.Rad2Deg; //get that ang

        float angDiff = Mathf.DeltaAngle(turretAng, predTargAng);
        if (Mathf.Abs(angDiff) < a) {
            return true;
        }
        return false;
    }

    public void ValidFire() {
        if (distance <= targetRange & TargetWithinAng(10)) {
            Child_Gun.GetComponent<FixedGun>().Fire();
        }
    }

    public void Fire() {
        Child_Gun.GetComponent<FixedGun>().Fire();
    }

    public float GetGunAng() {
        Vector3 myGunDir = Child_Gun.transform.up;
        return Mathf.Atan2(myGunDir.y, myGunDir.x) * Mathf.Rad2Deg;
    }

    public void SetTarget(GameObject T) {
        Target = T;
    }

    void OnDrawGizmos() {
        if (debug) { 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, targetRange);

            //Gizmos.DrawSphere(predictedAimPos, 0.5f);
        }
    }
}
