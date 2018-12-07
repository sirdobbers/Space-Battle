using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour {

    protected GameObject Target;
    protected List<GameObject> TurretArray = new List<GameObject>();
    protected List<GameObject> TurretBaseArray = new List<GameObject>();

    void Start () {
        Target = GameObject.FindGameObjectWithTag("Ship");
        
        foreach (Transform child in transform) {
            if (child.gameObject.tag == "TurretBase") {
                TurretBaseArray.Add(child.gameObject);
            }
            if (child.gameObject.tag == "Turret") {
                TurretArray.Add(child.gameObject);
            }
        }
    }

    void Update() {

        // FIRE AND ROTATE TURRETS
        for (int i = 0; i < TurretBaseArray.Count; i++) {
            if (TurretBaseArray[i] == null) {
                TurretBaseArray.RemoveAt(i);
            }
            else {
                Turret T = TurretBaseArray[i].transform.GetChild(0).GetComponent<Turret>();
                T.SetTarget(Target);
                T.RotateTurretToTarget(true);
                T.ValidFire();
            }
        }

        for (int i = 0; i < TurretArray.Count; i++) {
            Turret T = TurretArray[i].GetComponent<Turret>();
            T.SetTarget(Target);
            T.RotateTurretToTarget(true);
            T.ValidFire();
        }
    }
}
