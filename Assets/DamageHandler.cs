using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

    float armor = 10;
    float health = 10;

    public GameObject DestroyEffectPrefab;

    void Update() {
        if (health <= 0) {
            GameObject go = Instantiate(DestroyEffectPrefab, transform.position, transform.rotation);
            //go.GetComponent<Particle_Impact>().Init(2, float scale);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg, float pen) {
        float armorDMG = Mathf.Clamp(dmg, 0, armor);
        health -= armorDMG * pen + Mathf.Max(0, dmg - armor);
        armor -= armorDMG;
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
