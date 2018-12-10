using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPrediction2 : MonoBehaviour {
    
    void Start() {}
    void Update() {}

    public Vector3 GetAimLocation(GameObject Target, GameObject Self, float bulletSpeed) {
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
        float s = bulletSpeed;

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
        //print(t);
        //print(Target.GetComponent<Ship>().GetVel());
        //print(Self.GetComponentInParent<Rigidbody2D>().velocity);

        return relPos + relVel * t;
    }


    /*
    public Vector3 GetAimLocation(Vector3 t_RelPos, Vector3 t_RelVel, float bulletSpeed) {
        float px = t_RelPos.x;
        float py = t_RelPos.y;
        float vx = t_RelVel.x;
        float vy = t_RelVel.y;
        float s = bulletSpeed;

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

        return t_RelPos - t_RelVel * t;
    }
    */

    /*
    double time_of_impact(double px, double py, double vx, double vy, double s) {
        double a = s * s - (vx * vx + vy * vy);
        double b = px * vx + py * vy;
        double c = px * px + py * py;

        double d = b * b + a * c;

        double t = 0;
        if (d >= 0) {
            t = (b + sqrt(d)) / a;
            if (t < 0)
                t = 0;
        }

        return t;
    }
    */
}

