using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    float alpha = 3;
    float startAlpha;
    bool active = true;

    public GameObject DieEffectPrefab;

    [HideInInspector] public float shield;
    [HideInInspector] public float startShield;
    [HideInInspector] public ShieldModule Controller;
    [HideInInspector] public GameObject Parent;

    void Start(){
        startAlpha = alpha;
    }

    public void Die() {
        active = false;
        alpha = 1f;
        gameObject.layer = 9; // background (cant collide with bullets)
        gameObject.transform.localScale = gameObject.transform.localScale * 0.5f;
        //Instantiate(DieEffectPrefab, transform.position, transform.rotation);
    }
    
    void Update() {
        transform.position = Parent.transform.position;
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
                gameObject.transform.localScale = gameObject.transform.localScale * 1.15f;
            }
            else {
                Destroy(gameObject);
            }
        }
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("Vector1_AD77814F", Mathf.Clamp(alpha, 0, 1));       
    }

    public void TakeDamage(float dmg) {
        shield -= dmg;
        alpha = startAlpha;
    }
}
