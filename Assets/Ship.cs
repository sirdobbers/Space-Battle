using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour {
    #region Variables
    public enum Control {
        BasicAI, AdvancedAI, Player, PlayerAlt, None
    }
    public string shipName = "No_Name";
    public Control control = Control.None;
    
    [HideInInspector] public List<GameObject> TurretArray = new List<GameObject>();
    [HideInInspector] public List<GameObject> FixedGunArray = new List<GameObject>();
    
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

        // RENDER TEAM COLOR
        if (gameObject.layer == 11) {gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);}
        else {gameObject.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 1);}
        
        if (control == Control.None) {print("Warning: ShipControl set to None");}

        //gameObject.GetComponent<SpriteRenderer>().sortingOrder = (int)transform.position.z;
    }

    protected void CUpdate() {

    }

    #region Getters & Setters
    public Vector3 GetPos() {
        return transform.position;
    }
    public void SetPos(Vector3 Pos) {
        transform.position = Pos;
    }

    public void PrintInfo() {
        print("\n -- == || " + shipName + " || == --");
        print("(Controlled by " + control + ")");
        print("Armament:");
        print("  Turret Count: " + TurretArray.Count);
        print("  FixedGun Count: " + FixedGunArray.Count);
        print("Defense:");
        if (gameObject.GetComponent<DamageHandler>() != null) {
            print("  HP: " + gameObject.GetComponent<DamageHandler>().GetHP());
            print("  Armor: " + gameObject.GetComponent<DamageHandler>().GetArmor());
        }
        if (gameObject.GetComponent<Ship_Movement>() != null) {
            print("Mobility:");
            print("  Acceleration: " + gameObject.GetComponent<Ship_Movement>().acceleration);
            print("  Rotation Speed: " + gameObject.GetComponent<Ship_Movement>().rotSpeed);
        }
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
