using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public enum Dir
    {
        forward, backward, right, left, alternateRL
    }
    public bool autoSpawn = true;
    public GameObject SpawnShipPrefab;
    public float shipSpawnCD = 30;
    public float spawnInterval = 0.5f;
    public float numShipsPerSpawn = 5;
    public Dir launchDir;
    public float launchSpeed = 0.1f;

    private bool active;
    private float spawnTimer = 25;
    private float spawnIntervalTimer = 0;
    private float off = 90;

    private Vector3 parentRot;
    
    void Start()
    {
        if (launchDir == Dir.forward) {
            off = 0;
        }
        else if (launchDir == Dir.backward) {
            off = -180;
        }
        else if (launchDir == Dir.left) {
            off = -90;
        }
        else if (launchDir == Dir.right) {
            off = 90;
        }
        else if (launchDir == Dir.alternateRL) {
            off = 90;
        }
    }
    
    void Update()
    {
        parentRot = transform.rotation.eulerAngles;

        if (active || autoSpawn == true) {
            HandleSpawner();
        }
    }

    public void StartSpawn() {
        if (active == false && spawnTimer > shipSpawnCD) {
            active = true;
        }
    }

    public void StopSpawn() {
        active = false;
        spawnIntervalTimer = 0;
    }

    public void HandleSpawner() {
        if (spawnTimer > shipSpawnCD + spawnInterval * numShipsPerSpawn) {
            spawnTimer = 0;
            active = false;
        }
        else if (spawnTimer > shipSpawnCD) {
            if (spawnIntervalTimer <= 0) {
                SpawnShip();
                spawnIntervalTimer = spawnInterval;
            }
            else {
                spawnIntervalTimer -= Time.deltaTime;
            }
        }

        spawnTimer += Time.deltaTime;
    }

    public void SpawnShip() {
        Vector3 rot = transform.rotation.eulerAngles;
        Quaternion newRot = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z + off));

        GameObject go = Instantiate(SpawnShipPrefab, transform.position, newRot);
        go.GetComponent<Ship_Movement>().SetVel(base.GetComponent<Ship_Movement>().GetVel() + go.transform.up * launchSpeed);
        go.layer = gameObject.layer;
        go.transform.GetChild(1).gameObject.layer = gameObject.layer;
        go.transform.GetChild(1).transform.GetChild(0).gameObject.layer = gameObject.layer;
        if (launchDir == Dir.alternateRL) {
            off = off * -1;
        }
    }
}
