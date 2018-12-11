using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleTargetter : MonoBehaviour
{

    public GameObject MisslePrefab;

    Ship myShip;
    List<GameObject> TargetList = new List<GameObject>();
    bool active = false;

    float range = 50;
    float launchSpeed = 2f;
    float launchCD = 5;
    float launchCDtimer = 0;
    float fireInterval = 0.15f;
    float fireIntervalTimer = 0f;
    //float targetInterval = 0.15f;
    //float targetTimer = 0f;
    int maxMissles = 10;
    int curMissleCount = 0;

    void Start() {
        myShip = gameObject.GetComponent<Ship>();
    }

    void Update() {
        HandleInput();
    }

    void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Space) && launchCDtimer <= 0 && active == false) {
            ScanForShips();
            if (TargetList.Count > 0) {
                active = true;
            }
            else {
                active = false;
            }
        }
        else {
            launchCDtimer -= Time.deltaTime;
        }

        if (active) {
            FireMissles();
        }
    }

    void ScanForShips() {
        TargetList.Clear();
        GameObject[] TempArray = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject go in TempArray) {
            if (go.GetComponent<Ship>() != null && !go.Equals(this.gameObject)) {
                if (go.GetComponent<Ship>().gameObject.layer != gameObject.layer) {
                    if (Vector3.Distance(transform.position, go.transform.position) <= range) {
                        if (TargetList.Count < maxMissles) {
                            TargetList.Add(go);
                        }
                    }
                }
            }
        }
    }

    void RemoveNullShips() {
        for (int i = TargetList.Count - 1; i >= 0; i--) {
            if (TargetList[i] == null) {
                TargetList.RemoveAt(i);
            }
        }
    }

    void FireMissles() {
        RemoveNullShips();
        if (curMissleCount < TargetList.Count) {
            if (fireIntervalTimer <= 0) {
                SpawnMissle();
                fireIntervalTimer = fireInterval;
                curMissleCount++;
            }
            else {
                fireIntervalTimer -= Time.deltaTime;
            }
        }
        else {
            Reset();
        }
    }

    void SpawnMissle() {
        Vector3 rot = transform.rotation.eulerAngles;
        Quaternion newRot = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z + Random.Range(-20f, 20f)));
        GameObject go = Instantiate(MisslePrefab, transform.position, newRot);
        Missle missle = go.GetComponent<Missle>();
        missle.SetTarget(TargetList[curMissleCount]);
        Vector3 vel = myShip.GetVel() + transform.up * launchSpeed * Time.deltaTime;
        go.layer = gameObject.layer;
        missle.SetVals(30, 0, 4f, 5f, 10, 100, vel);
    }

    void Reset() {
        curMissleCount = 0;
        active = false;
        fireIntervalTimer = 0;
        launchCDtimer = launchCD;
        TargetList.Clear();
    }
}