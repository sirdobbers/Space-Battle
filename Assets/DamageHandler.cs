﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {
    public float health = 1;
    public float armor = 1;

    public GameObject DestroyEffectPrefab;
    public bool canTakeDmg = true;

    private HealthBar HPBar;
    private float startShield;
    private float startArmor;
    private float startHealth;

    void Start() {
        
        startArmor = armor;
        startHealth = health;

        foreach (Transform child in transform) {
            if (child.gameObject.name == "HealthBar") {
                HPBar = child.GetComponent<HealthBar>();
            }
        }
    }

    void Update() {
        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        GameObject go = Instantiate(DestroyEffectPrefab, transform.position, transform.rotation);
        Vector3 scale = gameObject.transform.localScale;
        go.transform.localScale = scale * 0.4f;
        go.transform.GetChild(0).transform.localScale = scale * 0.4f;

        //go.GetComponent<Particle_Impact>().Init(2, float scale);
        Destroy(gameObject);
    }
    

    public void TakeDamage(float dmg, float pen) {
        if (canTakeDmg) {
            float armorDMG = Mathf.Clamp(dmg, 0, armor);
            health -= armorDMG * pen + Mathf.Max(0, dmg - armor);
            armor -= armorDMG;
            

            if (HPBar != null) {
                HPBar.Hit();
            }
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
