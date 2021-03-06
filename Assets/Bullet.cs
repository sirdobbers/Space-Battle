﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float dmg = 4f;
    private float pen = 0f; //0 no pen ... 1 is full pen
    private float speed = 6f; // per second
    private float acc = 0.0f;
    private float travelTime = 10; //seconds
    
    Vector3 offsetVel; //set via SetTarget by other classes that fire bullets
    ContactPoint2D[] cp = new ContactPoint2D[10];
    float lifeTimeTimer;

    public GameObject ImpactEffectPrefab; //set prefab in inspector

    void Start() {
        gameObject.GetComponent<TrailRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        gameObject.GetComponent<TrailRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        GetComponent<TrailRenderer>().startWidth = Mathf.Clamp(0.05f * dmg, 0.01f, 0.5f);
        GetComponent<TrailRenderer>().time = Mathf.Clamp(dmg*0.2f, 0.1f, 4); ;
    }

    void OnTriggerEnter2D(Collider2D HitCollider) {
        DamageHandler DH = HitCollider.gameObject.GetComponent<DamageHandler>();
        if (DH != null) {
            DH.TakeDamage(dmg+dmg*Random.Range(-0.1f,0.1f), pen);
        }
        Shield S = HitCollider.gameObject.GetComponent<Shield>();
        if (S != null) {
            S.TakeDamage(dmg + dmg * Random.Range(-0.1f, 0.1f));
        }
        Explode();
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

    public void SetVals(float dmg, float pen, float speed, float acc, float travelTime, Vector3 trailColor) {
        this.dmg = dmg;
        this.pen = pen;
        this.speed = speed;
        this.acc = acc;
        this.travelTime = travelTime;
        trailColor = trailColor / 255;
        gameObject.GetComponent<TrailRenderer>().startColor = new Color(trailColor.x, trailColor.y, trailColor.z,1);
        gameObject.GetComponent<TrailRenderer>().endColor = new Color(trailColor.x, trailColor.y, trailColor.z, 0);
    }
}
