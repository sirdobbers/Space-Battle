using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg : MonoBehaviour
{

    public float moveMultiplier = 0.9f;
    GameObject target;
    Vector3 initPos;
    
    void Start()
    {
        target = Camera.main.gameObject;
        initPos = gameObject.transform.position;
    }
    
    void Update()
    {
        transform.position = initPos + target.transform.position * moveMultiplier;
    }
}
