using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Movement : MonoBehaviour
{
    public float acceleration = 0.1f; // units per second
    public float rotSpeed = 180f; // max rot speed in degrees per second
    public float dampening = 0.005f;
    float rotateP = 0;
    float rotateA = 360; //rot acceleration in degrees per sec
    float rotateB = 5; //rotate brake multiplier

    protected Vector3 vel = new Vector3(0, 0, 0);

    Quaternion QRot;
    Vector3 myDir;
    float myAng;

    // thrust variables just for other things to know which direction the ship is trying to go
    [HideInInspector] public float forward;
    [HideInInspector] public float rotate;
    [HideInInspector] public float strafe;
    [HideInInspector] public float boost;


    void Start()
    {

    }
    
    void Update()
    {
        boost = 0; strafe = 0; rotate = 0; forward = 0;

        QRot = transform.rotation;
        myDir = transform.up; myDir.Normalize();
        myAng = Mathf.Atan2(myDir.y, myDir.x) * Mathf.Rad2Deg;
    }

    public void Translate(float forward, float strafe, float boost) {
        this.forward = forward; this.strafe = strafe; this.boost = boost;
        Vector3 accVec = new Vector3(strafe, forward * (1 + boost), 0) * acceleration * Time.deltaTime;
        vel += QRot * accVec - vel * dampening;
        SetPos(GetPos() + vel);
    }

    // this should probably be touched up or moved to player control
    public void WorldTranslate(Vector3 dir, bool active, float boost, float strafe) {
        float dirAng = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float angDiff = Mathf.DeltaAngle(dirAng, myAng);
        if (angDiff < 20 && active == true) {
            Translate(1, strafe, boost);
        }
        else {
            Translate(0, strafe, boost);
        }
    }

    // ROTATION METHODS
    public void RotateToTarget(GameObject t) {
        if (t != null) {
            RotateToPosition(t.transform.position);
        }
    }
  
    public void RotateToDirection(Vector3 dir) {
        RotateToPosition(transform.position + dir);
    }

    public void RotateToPosition(Vector3 pos) {
        Vector3 posDir = pos - transform.position; posDir.Normalize();
        float posAng = Mathf.Atan2(posDir.y, posDir.x) * Mathf.Rad2Deg;
        float angDiff = Mathf.DeltaAngle(posAng, myAng);
        rotate = Mathf.Clamp(angDiff, -1, 1);

        Quaternion desiredRot = Quaternion.Euler(0, 0, posAng - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    public void Rotate(float rotate) {
        Quaternion QRot = transform.rotation;
        this.rotate = rotate;
        float z = QRot.eulerAngles.z;
        if (rotate != 0) {
            rotateP = Mathf.Clamp(rotateP + rotate*rotateA*Time.deltaTime, -rotSpeed, rotSpeed);
        }
        else {
            rotateP -= Mathf.Clamp(rotateP * Time.deltaTime*5, -rotSpeed, rotSpeed);
        }
        z -= rotateP * Time.deltaTime;
        QRot = Quaternion.Euler(0, 0, z);
        transform.rotation = QRot;
    }

    public Vector3 GetPos() {
        return transform.position;
    }
    public void SetPos(Vector3 Pos) {
        transform.position = Pos;
    }

    public Vector3 GetVel() {
        return vel;
    }

    public void SetVel(Vector3 v) {
        vel = v;
    }
}
