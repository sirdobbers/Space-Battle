using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Modular Turret script -> prefab
 *      - Place Gun on object
 *      - Set its type through its child prefab "fixedGun" (gun, missle, laser)
 * 
 * 
 */

public class FixedGun : MonoBehaviour {

    public enum TurretType
    {
        Gun, Missle, Laser
    }
    public TurretType type;
    public float projectileDamage = 4f;
    public float projectilePenetration = 0f;
    public float projectileInitSpeed = 6f;
    public float projectileAcceleration = 0.2f;
    public float projectileFlightTime = 10f;
    public float fireDelay = 0.5f;
    public GameObject MisslePrefab; //eventually remove the need for this
    public GameObject BulletPrefab; //eventually remove the need for this

    GameObject Parent_Ship;
    GameObject MissleTarget;
    
    Vector3 fireOffset = new Vector3(0f, 0.4f, 0f);
    Vector3 vel;
    
    float cooldownTimer = 0;
    
    private List<Collider2D> colliders = new List<Collider2D>();

    void Start() {
        if (transform.parent.tag == "Turret") {
            Vector3 scale = Vector3.Scale(transform.parent.transform.parent.transform.localScale, transform.parent.transform.localScale);
            fireOffset = Vector3.Scale(fireOffset, scale);
            Parent_Ship = transform.parent.gameObject.transform.parent.gameObject;
        }
        else {
            fireOffset = Vector3.Scale(fireOffset, transform.parent.transform.localScale);
            Parent_Ship = transform.parent.gameObject;
        }
        
        //set physics layer and local render layer
        gameObject.layer = Parent_Ship.layer;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerID=Parent_Ship.GetComponent<SpriteRenderer>().sortingLayerID;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    //not working atm dunno why (supposed to detect if object enter child collider2D component)
    void OnTriggerEnter2D(Collider2D HitCollider) {
        if (!colliders.Contains(HitCollider)) { colliders.Add(HitCollider); }
        for (int i = 0; i < colliders.Count; i++) {
            if (MissleTarget == null) {
                MissleTarget = colliders[0].gameObject;
            }
            else if (Vector3.Distance(GetPos(), colliders[0].gameObject.GetComponent<Ship>().GetPos())
                < Vector3.Distance(GetPos(), MissleTarget.GetComponent<Ship>().GetPos())) {
                MissleTarget = colliders[0].gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D HitCollider) {
        colliders.Remove(HitCollider);
    }

    void Update () {
        if (cooldownTimer <= 0) {
            cooldownTimer = fireDelay;
        }
        else {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public void Fire() {
        if (ReadyFire()) {
            if (Parent_Ship != null) { vel = Parent_Ship.GetComponent<Ship>().GetVel(); }
            Vector3 offsetPos = transform.rotation * fireOffset + vel;
            if (type == TurretType.Missle) {
                GameObject missle = (GameObject)Instantiate(MisslePrefab, transform.position + offsetPos, transform.rotation);
                missle.GetComponent<Missle>().SetOffsetVel(vel);
                if (MissleTarget != null) {missle.GetComponent<Missle>().SetTarget(MissleTarget);}
                missle.layer = gameObject.layer;
                missle.GetComponent<Missle>().SetVals(projectileDamage, projectilePenetration, projectileInitSpeed, projectileAcceleration, projectileFlightTime);
            }
            else if (type == TurretType.Gun) {
                GameObject bullet = (GameObject)Instantiate(BulletPrefab, transform.position + offsetPos, transform.rotation);
                bullet.GetComponent<Bullet>().SetOffsetVel(vel);
                bullet.layer = gameObject.layer;
                bullet.GetComponent<Bullet>().SetVals(projectileDamage, projectilePenetration, projectileInitSpeed, projectileAcceleration, projectileFlightTime);
            }
        }
    }

    public bool ReadyFire() {
        if (cooldownTimer <= 0) {
            return true;
        }
        return false;
    }

    Vector3 GetPos() {
        return transform.position;
    }
}
