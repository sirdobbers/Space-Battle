using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScanner : MonoBehaviour
{

    public enum ScanType
    {
        Enemy, Ally, All
    }
    private ScanType scanType = ScanType.Enemy;
    private float scanRange = 0; //0 means infinite range

    [HideInInspector] public List<GameObject> ShipList = new List<GameObject>();
    [HideInInspector] public GameObject ClosestShip;

    void Start() {

    }

    void Update() {
    }

    public List<GameObject> ScanForShips(ScanType scanType) {
        this.scanType = scanType;
        ScanForShips();
        return ShipList;
    }

    public List<GameObject> ScanForShipsInRange(ScanType scanType, float scanRange) {
        this.scanType = scanType;
        this.scanRange = scanRange;
        ScanForShips();
        return ShipList;
    }

    public GameObject ScanForClosestShip(ScanType scanType) {
        this.scanType = scanType;
        ScanForShips();
        FindClosestShip();
        return ClosestShip;
    }

    public void ScanForShips() {
        ShipList.Clear();
        GameObject[] TempArray = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject go in TempArray) {
            if (go.GetComponent<Ship>() != null && !go.Equals(this.gameObject)) {
                int targetLayer = go.GetComponent<Ship>().gameObject.layer;
                if (scanType == ScanType.Enemy && targetLayer != gameObject.layer) {
                    AddShip(go);
                }
                else if (scanType == ScanType.Ally && targetLayer == gameObject.layer) {
                    AddShip(go);
                }
                else if (scanType == ScanType.All) {
                    AddShip(go);
                }
            }
        }
    }

    private void AddShip(GameObject go) {
        if (scanRange == 0) {
            ShipList.Add(go);
        }
        else if (Vector3.Distance(transform.position, go.transform.position) <= scanRange) {
            ShipList.Add(go);
        }
    }

    public void RemoveNullShips() {
        for (int i = ShipList.Count - 1; i >= 0; i--) {
            if (ShipList[i] == null) {
                ShipList.RemoveAt(i);
            }
        }
    }

    public void FindClosestShip() {
        if (ShipList.Count > 0) {
            ClosestShip = ShipList[0];
            for (int i = 1; i < ShipList.Count; i++) {
                float targDist = Vector3.Distance(transform.position, ClosestShip.transform.position);
                float newDist = Vector3.Distance(transform.position, ShipList[i].transform.position);
                if (newDist < targDist) {
                    ClosestShip = ShipList[i];
                }
            }
        }
    }
}