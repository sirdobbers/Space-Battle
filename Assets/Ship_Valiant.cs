using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship_Valiant : Ship {

    float shieldLength = 5;
    float shieldCooldown = 20;
    float shieldTimer = 0;
    bool shieldActive = false;

    public GameObject ShieldEffectPrefab;
    GameObject shield;

    void Start() {
        base.CStart();
    }
	
	void Update () {
        base.CUpdate();

        //HandleShield();
	}

    public void HandleShield() {
        if (Input.GetKeyDown(KeyCode.X) & shieldTimer <= 0) {
            shieldActive = true;
            shieldTimer = shieldCooldown;
            shield = Instantiate(ShieldEffectPrefab, transform);
        }
        else {
            shieldTimer -= Time.deltaTime;
            if (shieldActive & shieldTimer < shieldCooldown - shieldLength) {
                shieldActive = false;
                Destroy(shield);
            }
        }

        GetComponent<DamageHandler>().canTakeDmg = !shieldActive;
    }

    /* HANDLE GUNS AND MOVE
    void HandleGuns() {
        if (base.control == Control.Player) {
            // FIRE FIXED GUNS
            if (Input.GetMouseButton(0)) {
                for (int i = 0; i < base.FixedGunArray.Count; i++) {
                    base.FixedGunArray[i].GetComponent<FixedGun>().Fire();
                }
            }
            // FIRE TURRETS
            if (Input.GetMouseButton(1)) {
                for (int i = 0; i < base.TurretArray.Count; i++) {
                    base.TurretArray[i].transform.GetChild(0).GetComponent<FixedGun>().Fire();
                }
            }
            // ROTATE TURRETS
            for (int i = 0; i < base.TurretArray.Count; i++) {
                base.TurretArray[i].GetComponent<Turret>().RotateTurretToMouseLocation();
            }
        }
        else if (control == Control.AI) {
            // FIRE FIXED GUNS (AI)
            FixedGun Gun;
            for (int i = 0; i < base.FixedGunArray.Count; i++) {
                Gun = FixedGunArray[i].GetComponent<FixedGun>();
                float angDiff = Mathf.DeltaAngle(base.GetTargetAng(), Gun.GetAng());
                if (Mathf.Abs(angDiff) < 5f) {
                    Gun.Fire();
                }
            }
            // FIRE TURRETS (AI)
            for (int i = 0; i < TurretArray.Count; i++) {
                Gun = TurretArray[i].transform.GetChild(0).GetComponent<FixedGun>();
                float angDiff = Mathf.DeltaAngle(Gun.GetAng(), base.GetTargetAng());
                if (Mathf.Abs(angDiff) < 5f) {
                    Gun.Fire();
                }
                // TURN TURRETS (AI)
                TurretArray[i].GetComponent<Turret>().RotateTurretToTargetPrediction(base.Target);
            }
        }
    }

    void Move() {
        float forward;
        float rotate;
        float strafe;
        float boost;
        if (control == Control.Player) {
            forward = Input.GetAxis("Throttle");
            rotate = Input.GetAxis("Turn");
            strafe = Input.GetAxis("Strafe");
            boost = Input.GetAxis("Boost");

            base.Translate(forward, strafe, boost);
            base.Rotate(rotate);
        }
        else if (control == Control.AI) {
            base.RotateToTarget();
            base.Translate(1, 0, 0);
        }
    }
    */
}
