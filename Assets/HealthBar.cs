using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    DamageHandler DamageScript;

    GameObject Parent;
    float Scale;
    //float startHP;
    //float startArmor;
    Image HPBar;
    Image ArmorBar;
    Image BGBar;

    public float alpha;
    float timeTillInvis = 10f; //seconds
    float timer = 0f;

    // Use this for initialization
    void Start () {
        Parent = transform.parent.gameObject;
        Scale = Parent.transform.localScale.magnitude;
        HPBar = transform.Find("Background").Find("Health").GetComponent<Image>();
        ArmorBar = transform.Find("Background").Find("Armor").GetComponent<Image>();
        BGBar = transform.Find("Background").GetComponent<Image>();
        DamageScript = Parent.GetComponent<DamageHandler>();
        //startHP = DamageScript.GetHP();
        //startArmor = DamageScript.GetArmor();

        gameObject.GetComponent<Canvas>().sortingLayerName = "UI";

        timer = timeTillInvis;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Parent.transform.position + new Vector3(0, 0.4f * Scale, 0);
        transform.rotation = new Quaternion(0,0,0,1);

        if (timer <= timeTillInvis) {
            alpha = (1 - (1 / timeTillInvis) * (timer*4 - timeTillInvis*2));
            HPBar.color = new Color(HPBar.color.r, HPBar.color.g, HPBar.color.b, alpha);
            ArmorBar.color = new Color(ArmorBar.color.r, ArmorBar.color.g, ArmorBar.color.b, alpha);
            BGBar.color = new Color(BGBar.color.r, BGBar.color.g, BGBar.color.b, alpha);
            timer+= 1 * Time.deltaTime;
        }
        
    }

    public void Hit() {
        float curHP = DamageScript.GetHP();
        float curArmor = DamageScript.GetArmor();
        float startHP = DamageScript.GetStartHP();
        float startArmor = DamageScript.GetStartArmor();
        float fillValue_HP = curHP / startHP;
        float fillValue_Armor = curArmor / startArmor;
        HPBar.fillAmount = fillValue_HP;
        ArmorBar.fillAmount = fillValue_Armor;
        timer = 0;
    }
}
