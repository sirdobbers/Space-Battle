using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldModule : MonoBehaviour
{
    public bool enable = true;
    public float shield = 100;

    public GameObject shieldPrefab;

    private GameObject shieldOb;


    void Start() {

    }

    void Activate() {
        if (shieldOb == null) {
            shieldOb = Instantiate(shieldPrefab, transform.position, transform.rotation);
            Shield shieldComp = shieldOb.GetComponent<Shield>();
            if (shieldComp != null) {
                shieldComp.Controller = GetComponent<ShieldModule>();
                shieldComp.Parent = gameObject;
                shieldComp.startShield = shield;
                shieldComp.shield = shield;
                shieldOb.transform.localScale = transform.localScale * 3;
                shieldOb.layer = gameObject.layer;
            }
        }
    }

    void Update() {
        if (enable) {
            if (Input.GetKeyDown(KeyCode.X)) {
                Activate();
            }
        }
    }
}
