using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Big : Ship {

    public GameObject SpawnShipPrefab;

    float shipSpawnCD = 30;
    float spawnTimer = 25;
    float spawnTimer2 = 0;
    float off = 90;

    void Start () {
        base.CStart();
    }
	
	// Update is called once per frame
	void Update () {
        base.CUpdate();
        base.AdvancedAIMovementControl();
        base.GenericAITurretControl();
        
        SpawnShips();
    }

    void SpawnShips() {
        
        Vector3 rot = transform.rotation.eulerAngles;
        Quaternion R = Quaternion.Euler(new Vector3(rot.x, rot.y, rot.z + off));

        
        if (spawnTimer > shipSpawnCD + 5) {
            spawnTimer = 0;
        }
        else if (spawnTimer > shipSpawnCD){
            if (spawnTimer2 > 1) {
                GameObject go = Instantiate(SpawnShipPrefab, transform.position, R);
                go.GetComponent<Ship>().SetVel(base.GetComponent<Ship>().GetVel());
                go.layer = gameObject.layer;
                go.transform.GetChild(1).gameObject.layer = gameObject.layer;
                go.transform.GetChild(1).transform.GetChild(0).gameObject.layer = gameObject.layer;
                off = off * -1;
                spawnTimer2 = 0;
            }
            else {
                spawnTimer2 += Time.deltaTime;
            }
        }

        spawnTimer += Time.deltaTime;
    }
}
