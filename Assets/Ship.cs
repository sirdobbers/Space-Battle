using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour {
    #region Variables
    public enum Control {
        AI, Player, GenericPlayer, GenericAI
    }

    public string shipName = "No_Name";
    public Control control = Control.AI;
    public float acceleration = 0.1f; // units per second
    public float rotSpeed = 360f; // degrees per second
    public float dampening = 0.005f;
    public float hp = 10f;
    public float armor = 10f;
    public float searchCooldown = 5f;
    public float maxRange = 50f;

    protected GameObject Target;
    protected List<GameObject> TargetArray = new List<GameObject>();
    protected List<GameObject> TurretArray = new List<GameObject>();
    protected List<GameObject> FixedGunArray = new List<GameObject>();


    protected Quaternion QRot;
    protected Vector3 vel = new Vector3(0, 0, 0);
    protected Vector3 targDir;
    protected Vector3 myDir;
    protected float targAng;
    protected float targDist;
    protected float myAng;
    protected float targAngDiff;
    protected float searchTimer;

    // THRUST DIRECTIONS USED FOR THRUST CLASS
    private float forward;
    private float rotate;
    private float strafe;
    private float boost;
    #endregion

    void Start() {CStart();}
    void Update() {CUpdate();}

    protected void CStart() {
        // ADD TURRET AND GUNS TO ARRAYS
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "Turret") {
                TurretArray.Add(child.gameObject);
            }
            if (child.gameObject.tag == "FixedGun") {
                FixedGunArray.Add(child.gameObject);
            }
        }

        if (gameObject.layer == 11) {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);
        }
        else {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 1);
        }

        //gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)transform.position.z;
    }

    protected void CUpdate() {
        boost = 0; strafe = 0; rotate = 0; forward = 0;

        HandleSearch();

        // SET GENERIC MOVEMENT CONTROLS IF CONTORL IS GENERIC
        if (control == Control.GenericPlayer) {
            GenericPlayerControl();
        }
        else if (control == Control.GenericAI) {
            GenericAIMovementControl();
            GenericAITurretControl();
        }

        // DIRECTION AND ANGLE VARIABLES TO BE USED THROUGHOUT
        if (Target != null) {
            targDir = Target.transform.position - transform.position; targDir.Normalize();
            targAng = Mathf.Atan2(targDir.y, targDir.x) * Mathf.Rad2Deg;
            myDir = transform.up; myDir.Normalize();
            myAng = Mathf.Atan2(myDir.y, myDir.x) * Mathf.Rad2Deg;
            targAngDiff = Mathf.DeltaAngle(targAng, myAng);
        }

        QRot = transform.rotation;
    }

    protected void GenericPlayerControl() {
        // FIRE FIXED GUNS
        if (Input.GetMouseButton(0)) {
            for (int i = 0; i < FixedGunArray.Count; i++) {
                FixedGunArray[i].GetComponent<FixedGun>().Fire();
            }
        }

        // FIRE TURRETS
        if (Input.GetMouseButton(1)) {
            for (int i = 0; i < TurretArray.Count; i++) {
                TurretArray[i].GetComponent<Turret>().Fire();
            }
        }
        // TURN TURRETS
        for (int i = 0; i < TurretArray.Count; i++) {
            TurretArray[i].GetComponent<Turret>().RotateTurretToMouseLocation();
        }

        //MOVEMENT
        forward = Input.GetAxis("Throttle");
        rotate = Input.GetAxis("Turn");
        strafe = Input.GetAxis("Strafe");
        boost = Input.GetAxis("Boost");

        Translate(forward, strafe, boost);
        Rotate(rotate);
    }
    protected void GenericAIMovementControl() {
        // MOVEMENT
        RotateToTarget();
        Translate(1*Random.Range(0.5f,1.5f), 0, 0);
    }
    protected void GenericAITurretControl() {
        // FIRE GUNS
        if (Mathf.Abs(targAngDiff) < 5) {
            for (int i = 0; i < FixedGunArray.Count; i++) {
                FixedGunArray[i].GetComponent<FixedGun>().Fire();
            }
        }
        // ROTATE & FIRE TURRETS
        for (int i = 0; i < TurretArray.Count; i++) {
            if (Target != null) {
                Turret T = TurretArray[i].GetComponent<Turret>();
                T.SetTarget(Target);
                T.RotateTurretToTarget(true);
                T.ValidFire();
            }
        }
    }
    protected void AdvancedAIMovementControl() {
        //Vector3 targDir = base.targDir;
        //Vector3 targDir = base.Target.GetComponent<Ship>().GetVel();
        //Vector3 newDir = Vector3.Cross(targDir, new Vector3(0, 0, 1));

        if (Target != null) {
            if (targDist > maxRange) {
                RotateToTarget();
            }
            else if (targDist < (maxRange / 4f)) {
                Vector3 newDir = Vector3.Cross(targDir, new Vector3(0, 0, 1));
                RotateToPosition(transform.position + newDir);
            }
            else {
                Vector3 targVel = Target.GetComponent<Ship>().GetVel();
                Vector3 relVel = targVel - GetVel();
                Vector3 newDir = relVel;
                RotateToPosition(transform.position + newDir);
            }
        }
        Translate(1, 0, 0);
    }

    protected void HandleSearch() {
        //searchTimer for ships and set target to nearest ship that is of another team
        if (searchTimer <= 0) {
            FindShips();
            searchTimer = searchCooldown + searchCooldown * Random.Range(-0.1f, 0.1f);
        }
        else {
            searchTimer -= Time.deltaTime;
        }
        if (Target == null & TargetArray.Count > 0) {
            FindShips();
        }
    }
    protected void FindShips() {
        GameObject[] TempArray = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject go in TempArray) {
            if (go.GetComponent<Ship>() != null & !go.Equals(this.gameObject)) {
                if (go.GetComponent<Ship>().gameObject.layer != gameObject.layer) {
                    TargetArray.Add(go);
                }
            }
        }
        if (TargetArray.Count > 0) {
            Target = TargetArray[0];

            for (int i = 0; i < TargetArray.Count; i++) {
                GameObject go = TargetArray[i];
                if (go == null) {
                    TargetArray.Remove(go);
                }
                else if (Target == null) {
                    Target = go;
                }
                else {
                    targDist = Vector3.Distance(Target.transform.position, transform.position);
                    float newTempDist = Vector3.Distance(go.transform.position, transform.position);

                    if (newTempDist < targDist) {
                        Target = go;
                    }
                }
            }
        }
        //print(name + ":");
        //print(TargetArray.Count);
    }

    #region Movement Methods
    // TRANSLATION METHOD
    public void Translate(float forward, float strafe, float boost) {
        this.forward = forward; this.strafe = strafe; this.boost = boost;
        Vector3 accVec = new Vector3(strafe, forward * (1 + boost), 0) * acceleration * Time.deltaTime;
        vel += QRot * accVec - vel * dampening;
        SetPos(GetPos() + vel);
    }
    
    // ROTATION METHODS (next 3 methods)
    public void RotateToPosition(Vector3 pos) {
        Vector3 posDir = pos - transform.position; posDir.Normalize();
        float posAng = Mathf.Atan2(posDir.y, posDir.x) * Mathf.Rad2Deg;
        float posAngDiff = Mathf.DeltaAngle(posAng, myAng);
        rotate = Mathf.Clamp(posAngDiff, -1, 1);

        Quaternion desiredRot = Quaternion.Euler(0, 0, posAng - 90);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    public void RotateToTarget() {
        if (Target != null) {
            Quaternion desiredRot = Quaternion.Euler(0, 0, targAng - 90);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
            rotate = Mathf.Clamp(targAngDiff, -1, 1);
        }
    }

    public void Rotate(float rotate) {
        this.rotate = rotate;
        float z = QRot.eulerAngles.z;
        z -= rotSpeed * Time.deltaTime * rotate;
        QRot = Quaternion.Euler(0, 0, z);
        transform.rotation = QRot;
    }
    #endregion

    #region Getters & Setters
    public float GetThrust(string dir) {
        if (dir == "forward") {return forward;}
        else if (dir == "strafe") {return strafe;}
        else if (dir == "boost") {return boost;}
        else if (dir == "rotate") {return rotate;}
        return 0f;
    }
    
    public GameObject GetTarget() {
        return Target;
    }
    public void SetTarget(GameObject T) {
        Target = T;
    }
    public float GetTargetAng() {
        return targAng;
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
        //print("  Target Scan Range: " + targetScanRange);
    }
    #endregion
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
