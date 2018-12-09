using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorDmgIndicator : MonoBehaviour {


    float startHP;
    Color startColor;

	// Use this for initialization
	void Start () {
        startHP = gameObject.GetComponent<DamageHandler>().GetHP();
        startColor = gameObject.GetComponent<SpriteRenderer>().color;

    }
	
	// Update is called once per frame
	void Update () {
        float curHP = gameObject.GetComponent<DamageHandler>().GetHP();
        float a = (curHP/startHP)*0.7f+0.3f;
        Color newColor = new Color(a, a, a)*startColor;
        //print(newColor);
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}
