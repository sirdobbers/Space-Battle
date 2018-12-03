using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BColor
    {
        green, blue, red
    }

    float dmg = 4f;
    float pen = 0f; //0 no pen ... 1 is full pen
    float speed = 6f; // per second
    float acc = 0.0f;
    float travelTime = 10; //seconds
    BColor bulletColor = BColor.blue;
    
    Vector3 offsetVel; //set via SetTarget by other classes that fire bullets
    ContactPoint2D[] cp = new ContactPoint2D[10];
    float lifeTimeTimer;

    public GameObject ImpactEffectPrefab; //set prefab in inspector

    void Start() {
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Front";
    }

    void OnTriggerEnter2D(Collider2D HitCollider) {
        if (HitCollider.gameObject.GetComponent<DamageHandler>() != null) {
            HitCollider.gameObject.GetComponent<DamageHandler>().TakeDamage(dmg, pen);
            Explode();
        }
    }
    
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
