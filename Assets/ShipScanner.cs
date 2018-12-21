using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScanner : MonoBehaviour
{

    public enum ScanType
    {
        Enemy, Ally, All
    }
    public bool autoScan = true;
    public ScanType scanType = ScanType.Enemy;
    public float scanRange = 0; //0 means infinite range

    [HideInInspector] public List<GameObject> ShipList = new List<GameObject>();
    [HideInInspector] public GameObject ClosestShip;

    private float scanInterval = 5;
    private float intervalTimer = 0;

    void Start() {

    }

    void Update() {
        if (autoScan == true) {
            if (intervalTimer <= 0) {
                Scan();
                intervalTimer = scanInterval;
            }
            else {
                intervalTimer -= Time.deltaTime;
            }
        }
    }

    public void ScanInRange(ScanType t, float scanRange) {
        this.scanRange = scanRange;
        Scan(t);
    }

    public void Scan() {
        Scan(scanType);
    }

    public void Scan(ScanType t) {
        ShipList.Clear();
        GameObject[] TempArray = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject go in TempArray) {
            if (go.GetComponent<Ship>() != null && !go.Equals(this.gameObject)) {
                int targetLayer = go.GetComponent<Ship>().gameObject.layer;
                if (t == ScanType.Enemy && targetLayer != gameObject.layer) {
                    AddShip(go);
                }
                else if (t == ScanType.Ally && targetLayer == gameObject.layer) {
                    AddShip(go);
                }
                else if (t == ScanType.All) {
                    AddShip(go);
                }
            }
        }
        FindClosestShip();
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

    public List<GameObject> GetShips() {
        RemoveNullShips();
        return ShipList;
    }

    public GameObject GetClosestShip() {
        RemoveNullShips();
        return ClosestShip;
    }
}