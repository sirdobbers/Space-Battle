using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public bool active = true;
    public float shield = 100;
    float shieldAlpha = 3;
    float startAlpha;

    public GameObject DieEffectPrefab;

    void Start()
    {
        startAlpha = shieldAlpha;
    }

    // Update is called once per frame
    void Update() {
        if (active) {
            if (shield <= 0) {
                gameObject.layer = 9;
                shieldAlpha = 0;
                active = false;
                Die();
            }
            else {
                gameObject.layer = 12;
            }
        }
        else{
            gameObject.layer = 9;
            shieldAlpha = 0;
        }

        if (shieldAlpha > 0) {
            shieldAlpha -= 2f * Time.deltaTime;
        }
        gameObject.GetComponent<MeshRenderer>().materials[0].SetFloat("Vector1_AD77814F", Mathf.Clamp(shieldAlpha, 0, 1));
    }

    public void TakeDamage(float dmg) {
        shield -= dmg;
        shieldAlpha = startAlpha;
    }

    public void Die() {
        GameObject go = Instantiate(DieEffectPrefab, transform.position, transform.rotation);
    }
}
