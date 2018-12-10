using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    float dmg = 10f;
    float pen = 0f; //0 no pen ... 1 is full pen
<<<<<<< HEAD
    float speed = 2f; // per second
    float acc = 5f; // per second
    float travelTime = 10; // seconds
    float maxSpeed = 40f;
=======
    float speed = 3f; // per second
    float acc = 0.1f; // per second
    float travelTime = 5; // seconds
>>>>>>> parent of 503e910... full scale battles,
    float explosionRadius = 50f;
    float rotSpeed = 50; // per second

    GameObject target = null; //set via SetTarget by other classes
    Vector3 offsetVel = new Vector3(0,0,0); //set via SetOffsetVel by other classes
    float lifeTimeTimer;
    ContactPoint2D[] cp = new ContactPoint2D[10];

    public GameObject ImpactEffectPrefab; //set prefab in inspector

    void OnTriggerEnter2D(Collider2D HitCollider) {
        HitCollider.gameObject.GetComponent<DamageHandler>().TakeDamage(dmg,pen);
        Explode();

        /* ****This can be used in the future for specific dmg decals on ships****
        if (HitCollider.GetComponent<EnemyShip>() != null) {
            HitCollider.GetContacts(cp);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, cp[0].normal);
            Vector3 pos = cp[0].point;
        }*/
    }

    void Start() {
        // set to render on top of everything else
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
    }

    void Update() {

        // Rotate toward target
        if (target != null) {
            Vector3 dir = target.transform.position - transform.position;
            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        }
<<<<<<< HEAD
    }

    void Move() {
        // rotate

        Vector3 targPos = gameObject.GetComponent<BulletPrediction2>().GetAimLocation(target, gameObject, speed * Time.deltaTime);
        Vector3 targDir = (transform.position + targPos) - transform.position;
        float targAng = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, targAng);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);

        // translate
        /*
        Rigidbody2D R = gameObject.GetComponent<Rigidbody2D>();
=======

        // Travel forward
>>>>>>> parent of 503e910... full scale battles,
        transform.Translate((new Vector3(0, speed, 0)) * Time.deltaTime);
        speed += acc;
        Vector3 Pos = transform.position;
        Pos += offsetVel;
        transform.position = Pos;
<<<<<<< HEAD
        */

        transform.Translate((new Vector3(0, speed - speed * dampening, 0)) * Time.deltaTime);
        if (speed < maxSpeed) { 
            speed += acc * Time.deltaTime;
        }

        Vector3 Pos = transform.position + offsetVel;
        transform.position = Pos;

        if (offsetVel.magnitude > 0) {
            offsetVel = offsetVel * 0.98f;
        }


        if (dampening > 0) {
            dampening -= Time.deltaTime * 0.3f;
        }
=======

        // Timer till auto explode
        if (lifeTimeTimer > travelTime) { Explode(); }
        else { lifeTimeTimer += Time.deltaTime; }
>>>>>>> parent of 503e910... full scale battles,
    }

    void Explode() {
        Instantiate(ImpactEffectPrefab, transform.position, transform.rotation); //this is an explosion effect

        //TODO: find ships in explosion radius
        //TODO: do dmg to them relative to distance

        Destroy(gameObject);
    }

    public void SetOffsetVel(Vector3 off) {
        offsetVel = off;
    }

    public void SetTarget(GameObject t) {
        target = t;
    }

    public void SetVals(float dmg, float pen, float speed, float acc, float travelTime) {
        this.dmg = dmg;
        this.pen = pen;
        this.speed = speed;
        this.acc = acc;
        this.travelTime = travelTime;
    }
}
