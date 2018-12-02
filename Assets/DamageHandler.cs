using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

    float armor = 10;
    float health = 10;

    void Update() {
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg, float pen) {
        if (armor > 0) {
            health -= dmg * pen;
            armor -= dmg;
        }
        else {
            health -= dmg;
        }
    }

    public void SetHP(float hp) {
        health = hp;
    }

    public float GetHP() {
        return health;
    }

    public void SetArmor(float a) {
        armor = a;
    }

    public float GetArmor() {
        return armor;
    }
}
