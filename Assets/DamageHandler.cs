using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {
    float startArmor;
    float startHealth;


    float armor;
    float health;

    public GameObject DestroyEffectPrefab;

    HealthBar HPBar;
    
    void Start() {
        armor = gameObject.GetComponent<Ship>().armor;
        health = gameObject.GetComponent<Ship>().hp;

        startArmor = armor;
        startHealth = health;

        HPBar = transform.GetComponentInChildren<HealthBar>();
    }

    void Update() {
        if (health <= 0) {
            GameObject go = Instantiate(DestroyEffectPrefab, transform.position, transform.rotation);
            Vector3 shipScale = gameObject.transform.localScale;
            go.transform.localScale = shipScale*0.4f;
            go.transform.GetChild(0).transform.localScale = shipScale*0.4f;
            //go.GetComponent<Particle_Impact>().Init(2, float scale);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg, float pen) {
        float armorDMG = Mathf.Clamp(dmg, 0, armor);
        health -= armorDMG * pen + Mathf.Max(0, dmg - armor);
        armor -= armorDMG;

        if (HPBar != null) {
            HPBar.Hit();
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

    public float GetStartArmor() {
        return startArmor;
    }

    public float GetStartHP() {
        return startHealth;
    }
}
