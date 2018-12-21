using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool enable = true;
    public float shield = 100;
    float alpha = 3;
    float startAlpha;
    float startShield;
    bool active = true;

    void Start(){
        startShield = shield;
        startAlpha = alpha;
        Activate();
    }

    void Activate() {
        active = true;
        alpha = startAlpha;
        shield = startShield;
        gameObject.layer = 12; //team 2 (can collide with bullets)
        transform.localScale = transform.parent.transform.localScale * 4;
    }

    public void Die() {
        active = false;
        alpha = 1f;
        gameObject.layer = 9; // background (cant collide with bullets)
        //GameObject go = Instantiate(DieEffectPrefab, transform.position, transform.rotation);
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            Activate();
        }
        if (active) {
            if (shield > 0) {
                if (alpha > 0) {
                    alpha -= 2f * Time.deltaTime;
                }
            }
            else {
                Die();
            }
        }
        else {
            if (alpha > 0) {
                alpha -= 6f * Time.deltaTime;
                gameObject.transform.localScale = gameObject.transform.localScale * 1.1f;
            }
        }
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("Vector1_AD77814F", Mathf.Clamp(alpha, 0, 1));       
    }

    public void TakeDamage(float dmg) {
        shield -= dmg;
        alpha = startAlpha;
    }
}
