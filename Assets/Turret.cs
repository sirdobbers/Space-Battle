using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Modular Turret script -> prefab
 *      - Place Turret on object
 *      - Set its type through its child prefab "fixedGun" (gun, missle, laser)
 * 
 * 
 * 
 */

public class Turret : MonoBehaviour {
    public float rotSpeed = 90f;

    GameObject Parent_Ship;
    GameObject Child_Gun;
    
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
}
