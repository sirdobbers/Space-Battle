using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    private float dmg = 10f;
    private float pen = 0f; //0 no pen ... 1 is full pen
    private float speed = 2f; // per second
    private float acc = 5f; // per second
    private float travelTime = 10; // seconds
    private float maxSpeed = 40f;
    private float explosionRadius = 5f;
    private float rotSpeed = 50; // per second
    private float dampening = 0.9f;
    private float lifeTimeTimer;

    private Vector3 vel = new Vector3(0, 0, 0);
    private GameObject target = null;
    private ShipScanner myScanner;
    private DamageHandler myDH;
    //ContactPoint2D[] cp = new ContactPoint2D[10];

    void Start() {
        myScanner = gameObject.GetComponent<ShipScanner>();
        myDH = gameObject.GetComponent<DamageHandler>();
    }

    void Update() {
        if (target != null & lifeTimeTimer <= travelTime) {
            Move();
            lifeTimeTimer += Time.deltaTime;
        }
        else {
            Explode();
        }
    }

    void OnTriggerEnter2D(Collider2D HitCollider) {
        DamageHandler DH = HitCollider.gameObject.GetComponent<DamageHandler>();
        if (DH != null) {
            DH.TakeDamage(dmg + dmg * Random.Range(-0.1f, 0.1f), pen);
        }

        Bullet B = HitCollider.gameObject.GetComponent<Bullet>();
        if (B == null) {
            Explode();
        }

        /* //This can be used in the future for specific dmg decals on ships
        if (HitCollider.GetComponent<EnemyShip>() != null) {
            HitCollider.GetContacts(cp);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, cp[0].normal);
            Vector3 pos = cp[0].point;
        }*/
    }

    void Move() {
        Vector3 targPos = gameObject.GetComponent<BulletPrediction>().GetAimLocation(target, gameObject, speed * Time.deltaTime);
        Vector3 targDir = (transform.position + targPos) - transform.position;
        float targAng = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, targAng);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
        
        transform.Translate((new Vector3(0, speed - speed * dampening, 0)) * Time.deltaTime);
        if (speed < maxSpeed) {
            speed += acc * Time.deltaTime;
        }

        Vector3 Pos = transform.position + vel;
        transform.position = Pos;

        if (dampening > 0) {
            dampening -= Time.deltaTime * 0.3f;
        }
    }

    void Explode() {
        // aoe dmg
        myScanner.ScanInRange(ShipScanner.ScanType.Enemy, explosionRadius);
        List<GameObject> ShipList = myScanner.GetShips();
        for (int i = 0; i< ShipList.Count; i++) {
            float shipDist = Vector3.Distance(transform.position, ShipList[i].transform.position);
            float aoeDMG = Mathf.Max(0, (explosionRadius - shipDist)/explosionRadius * dmg);
            ShipList[i].GetComponent<DamageHandler>().TakeDamage(aoeDMG, 0);
        }

        myDH.Die();
    }

    #region Getters Setters
    public Vector3 GetVel() {
        return vel;
    }

    public void SetVel(Vector3 vel) {
        this.vel = vel;
    }

    public void SetTarget(GameObject t) {
        target = t;
    }

    public void SetVals(float dmg, float pen, float speed, float acc, float travelTime, float rotSpeed, Vector3 vel) {
        this.dmg = dmg;
        this.pen = pen;
        this.speed = speed;
        this.acc = acc;
        this.travelTime = travelTime;
        this.rotSpeed = rotSpeed;
        this.vel = vel;
    }
    #endregion
}
