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

    private GameObject Parent_Ship;
    private GameObject Child_Gun;
    private Vector3 aimPos; //for predicted location
    
    void Start() {
        Parent_Ship = transform.parent.gameObject;
        Child_Gun = transform.GetChild(0).gameObject;

        //set physics layer and local render layer
        gameObject.layer = Parent_Ship.layer;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID = Parent_Ship.GetComponent<SpriteRenderer>().sortingLayerID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    void Update () {
        
    }
    
    public void RotateTurretToMouseLocation() {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Child_Gun.transform.position;
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle+180);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime); 
    }

    public void RotateTurretToTargetLocation(GameObject Target) {
        Vector3 dir = Target.transform.position - Child_Gun.transform.position;
        dir.Normalize();
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    public void RotateTurretToTargetPrediction(GameObject Target) {
        
        float bulletSpeed = Child_Gun.GetComponent<FixedGun>().projectileInitSpeed * Time.deltaTime;
        aimPos = gameObject.GetComponent<BulletPrediction2>().GetAimLocation(Target, gameObject, bulletSpeed);

        Vector3 dir = (transform.position + aimPos) - transform.position;
        dir.Normalize();
        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
        Child_Gun.transform.rotation = Quaternion.RotateTowards(Child_Gun.transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }
    
    //CHECKS TO SEE IF THE PREDICTED TARGET POSITION IS WITHIN A BEARING
    public bool TargetInAng(float a) {
        Vector3 turretDir = Child_Gun.transform.up; //get turret forward dir
        float turretAng = Mathf.Atan2(turretDir.y, turretDir.x) * Mathf.Rad2Deg; //get that ang

        Vector3 predTargDir = (transform.position + aimPos) - transform.position; predTargDir.Normalize(); //get pred target dir rel to gun
        float predTargAng = Mathf.Atan2(predTargDir.y, predTargDir.x) * Mathf.Rad2Deg; //get that ang

        float angDiff = Mathf.DeltaAngle(turretAng, predTargAng);
        if (Mathf.Abs(angDiff) < a) {
            return true;
        }
        return false;
    }
}
