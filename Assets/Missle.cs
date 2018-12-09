using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    float dmg = 10f;
    float pen = 0f; //0 no pen ... 1 is full pen
    float speed = 2f; // per second
    float acc = 5f; // per second
    float travelTime = 10; // seconds
    float explosionRadius = 50f;
    float rotSpeed = 50; // per second
    float dampening = 0.9f;

    Vector3 offsetVel = new Vector3(0,0,0);

    GameObject target = null;
    float lifeTimeTimer;
    ContactPoint2D[] cp = new ContactPoint2D[10];

    public GameObject ImpactEffectPrefab; //set prefab in inspector

    void OnTriggerEnter2D(Collider2D HitCollider) {
        if (HitCollider.gameObject.GetComponent<DamageHandler>() != null) {
            HitCollider.gameObject.GetComponent<DamageHandler>().TakeDamage(dmg + dmg * Random.Range(-0.1f, 0.1f), pen);
            Explode();
        }
        Explode();

        /* ****This can be used in the future for specific dmg decals on ships****
        if (HitCollider.GetComponent<EnemyShip>() != null) {
            HitCollider.GetContacts(cp);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, cp[0].normal);
            Vector3 pos = cp[0].point;
        }*/
    }

    void Start() {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
    }

    void Update() {
        if (target != null & lifeTimeTimer<=travelTime) {
            Move();
            lifeTimeTimer += Time.deltaTime;
        }
        else {
            Explode();
        }
    }
    
    void Move() {
        // rotate

        Vector3 targPos = gameObject.GetComponent<BulletPrediction2>().GetAimLocation(target, gameObject, speed);
        Vector3 targDir = (transform.position+targPos) - transform.position;
        float targAng = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, targAng);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);

        // translate
        /*
        Rigidbody2D R = gameObject.GetComponent<Rigidbody2D>();
        transform.Translate((new Vector3(0, speed, 0)) * Time.deltaTime);
        speed += acc;
        Vector3 Pos = transform.position;
        Pos += offsetVel;
        transform.position = Pos;
        */

        transform.Translate((new Vector3(0, speed-speed*dampening, 0)) * Time.deltaTime);
        speed += acc * Time.deltaTime;

        Vector3 Pos = transform.position + offsetVel;
        transform.position = Pos;

        if (dampening > 0) {
            dampening -= Time.deltaTime * 0.3f;
        }
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

    public void SetVals(float dmg, float pen, float speed, float acc, float travelTime, float rotSpeed) {
        this.dmg = dmg;
        this.pen = pen;
        this.speed = speed;
        this.acc = acc;
        this.travelTime = travelTime;
        this.rotSpeed = rotSpeed;
    }
}
