using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SHIP
 *      - Set all variables
 *      - Set Sorting layer (all children will join this layer)
 *      
 * 
 * 
 */


public class Ship : MonoBehaviour {
    public enum Control
    {
        AI, Player
    }

    // MOVEMENT VARIABLES
    public string shipName = "NoName";
    public Control control = Control.AI;
    public float acceleration = 0.1f; // units per second
    public float rotSpeed = 360f; // degrees per second
    private float dampening = 0.005f;
    public float hp = 10f;
    public float armor = 10f;
    public float targetScanRange = 100f;

    public List<GameObject> TurretArray = new List<GameObject>();
    public List<GameObject> FixedGunArray = new List<GameObject>();
    public List<GameObject> ShipArray = new List<GameObject>();

    private Vector3 vel = new Vector3(0, 0, 0);
    private Quaternion QRot;
    private GameObject Target_Ship;
    private float searchTimer; //use this to rescan every few seconds maybe

    //directions accessed via method for thruster objects
    float forward;
    float rotate;
    float strafe;
    float boost;

    void Start() {
        if (control == Control.Player) {
            gameObject.tag = "Player";
        }
        else {
            Target_Ship = GameObject.FindGameObjectWithTag("Player");
        }

        gameObject.GetComponent<DamageHandler>().SetHP(hp);
        gameObject.GetComponent<DamageHandler>().SetArmor(armor);

        // add turrets and guns to lists
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Turret") {
                TurretArray.Add(child.gameObject);
            }
            if (child.gameObject.tag == "FixedGun") {
                FixedGunArray.Add(child.gameObject);
            }
        }

        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //PrintInfo();
    }

    void OnTriggerEnter2D(Collider2D HitCollider) {
        //take dmg as a func of combined vel maybe and bounce off shit
    }

    void Update() {
        HandleGuns();
        Move();
    }

    public void Translate(float forward, float strafe, float boost) {
        QRot = transform.rotation;
        Vector3 accVec = new Vector3(strafe, forward *(1+boost), 0) * acceleration * Time.deltaTime;
        vel += QRot * accVec - vel * dampening;
        SetPos(GetPos() + vel);
    }

    public void Rotate(float turn) {
        QRot = transform.rotation;
        float z = QRot.eulerAngles.z;
        z -= rotSpeed * Time.deltaTime * turn;
        QRot = Quaternion.Euler(0, 0, z);
        transform.rotation = QRot;
    }

    void SearchForTargets() {
        //scan in range for enemy ships
        //target = closest ship
    }
    
    public GameObject GetTarget() {
        return Target_Ship;
    }

    public Vector3 GetPos() {
        return transform.position;
    }

    public void SetPos(Vector3 Pos) {
        transform.position = Pos;
    }

    public void SetPos(float x, float y) {
        transform.position = new Vector3(x, y, 0);
    }

    public Vector3 GetVel() {
        return vel;
    }

    public void PrintInfo() {
        print("\n -- == || " + shipName + " || == --");
        print("(Controlled by " + control + ")");
        print("Armament:");
        print("  Turret Count: " + TurretArray.Count);
        print("  FixedGun Count: " + FixedGunArray.Count);
        print("Defense:");
        print("  HP: " + gameObject.GetComponent<DamageHandler>().GetHP());
        print("  Armor: " + gameObject.GetComponent<DamageHandler>().GetArmor());
        print("Mobility:");
        print("  Acceleration: " + acceleration);
        print("  Rotation Speed: " + rotSpeed);
        print("Utility:");
        print("  Target Scan Range: " + targetScanRange);
    }

    void HandleGuns() {
        if (control == Control.Player) {
            //fire turrets and fixed guns
            if (Input.GetMouseButton(0)) {
                for (int i = 0; i < FixedGunArray.Count; i++) {
                    FixedGunArray[i].GetComponent<FixedGun>().Fire();
                }
            }
            if (Input.GetMouseButton(1)) {
                for (int i = 0; i < TurretArray.Count; i++) {
                    TurretArray[i].transform.GetChild(0).GetComponent<FixedGun>().Fire();
                }
            }
            //turn turrets
            for (int i = 0; i < TurretArray.Count; i++) {
                TurretArray[i].GetComponent<Turret>().RotateTurretToMouseLocation();
            }
        }
        else if (control == Control.AI) {
            //fire turrets and fixed guns
            for (int i = 0; i < FixedGunArray.Count; i++) {
                FixedGunArray[i].GetComponent<FixedGun>().Fire();
            }
            for (int i = 0; i < TurretArray.Count; i++) {
                TurretArray[i].transform.GetChild(0).GetComponent<FixedGun>().Fire();
            }
            //turn turrets
            for (int i = 0; i < TurretArray.Count; i++) {
                TurretArray[i].GetComponent<Turret>().RotateTurretToTargetPrediction(Target_Ship);
            }
        }
    }

    void Move() {
        if (control == Control.Player) {
            // Input definition
            forward = Input.GetAxis("Throttle");
            rotate = Input.GetAxis("Turn");
            strafe = Input.GetAxis("Strafe");
            boost = Input.GetAxis("Boost");

            Translate(forward, strafe, boost);
            Rotate(rotate);
        }
        else if (control == Control.AI) {
            //rotate ship at player
            Vector3 dir = Target_Ship.transform.position - transform.position;
            dir.Normalize();
            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);

            //translate ship
            forward = 1f;
            boost = 0;
            Translate(forward, 0, boost);
        }
    }

    //used by thurster script
    public float GetThrust(string dir) {
        if (dir == "forward") {
            return forward;
        }
        else if (dir == "strafe") {
            return strafe;
        }
        else if (dir == "boost") {
            return boost;
        }
        else if (dir == "rotate") {
            return rotate;
        }
        return 0f;
    }
}






/*// Trap ship within camera
        if (getPos().y > Height){
            SetPos(getPos().x, Height);
            vel = new Vector3(vel.x, 0, 0);
        }
        if (getPos().y < -Height){
            SetPos(getPos().x, -Height);
            vel = new Vector3(vel.x, 0, 0);
        }
        if (getPos().x > Width){
            SetPos(Width, getPos().y);
            vel = new Vector3(0, vel.y, 0);
        }
        if (getPos().x < -Width){
            SetPos(-Width, getPos().y);
            vel = new Vector3(0, vel.y, 0);
        }
        */
