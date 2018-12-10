using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrediction2 : MonoBehaviour {
    
    void Start() {}
    void Update() {}

    public Vector3 GetAimLocation(GameObject Target, GameObject Self, float speed) {
        Vector3 relPos = Target.transform.position - Self.transform.position;
        Vector3 myVel= new Vector3(0,0,0);
        if (Self.GetComponentInParent<Ship>() != null) {
            myVel = Self.GetComponentInParent<Ship>().GetVel();
        }
        myVel = Self.GetComponentInParent<Rigidbody2D>().velocity;
        Vector3 relVel = Target.GetComponent<Ship>().GetVel() - myVel;

        float px = relPos.x;
        float py = relPos.y;
        float vx = relVel.x;
        float vy = relVel.y;
        float s = speed;

        float a = s * s - (vx * vx + vy * vy);
        float b = px * vx + py * vy;
        float c = px * px + py * py;

        float d = b * b + a * c;

        float t = 0; //time to impact
        if (d >= 0) {
            t = (b + Mathf.Sqrt(d)) / a;
            if (t < 0)
                t = 0;
        }

        return relPos + relVel * t;
    }
}

