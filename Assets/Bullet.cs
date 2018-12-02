using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float dmg = 4f;
    float pen = 0f; //0 no pen ... 1 is full pen
    float speed = 6f; // per second
    float acc = 0.2f;
    float travelTime = 10; //seconds

    Vector3 vel;
    Vector3 offsetVel;

    float lifeTimeTimer;

    public GameObject ImpactEffectPrefab;
    ContactPoint2D[] cp = new ContactPoint2D[10];

    void Start() {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
    }

    void OnTriggerEnter2D(Collider2D HitCollider) {
        if (HitCollider.gameObject.GetComponent<DamageHandler>() != null) {
            HitCollider.gameObject.GetComponent<DamageHandler>().TakeDamage(dmg, pen);
            Explode();
        }
    }

    // Update is called once per frame
    void Update () {
        
        //move forward
        transform.Translate((new Vector3(0,speed,0))*Time.deltaTime);
        speed += acc;
        Vector3 Pos = transform.position;
        Pos += offsetVel;
        transform.position = Pos;

        // Timer till auto explode
        if (lifeTimeTimer > travelTime) { Explode(); }
        else{ lifeTimeTimer += Time.deltaTime; }
    }

    public void SetOffsetVel(Vector3 off) {
        offsetVel = off;
    }

    void Explode() {
        Instantiate(ImpactEffectPrefab, transform.position, transform.rotation); //this is an explosion effect

        //TODO: find ships in explosion radius
        //TODO: do dmg to them relative to distance

        Destroy(gameObject);
    }

    public void SetVals(float dmg, float pen, float speed, float acc, float travelTime) {
        this.dmg = dmg;
        this.pen = pen;
        this.speed = speed;
        this.acc = acc;
        this.travelTime = travelTime;
    }
}
