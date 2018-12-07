using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject Parent_Ship = transform.parent.gameObject;
        //gameObject.GetComponent<TrailRenderer>().sortingLayerID = Parent_Ship.GetComponent<SpriteRenderer>().sortingLayerID;
    }
	
	// Update is called once per frame
	void Update () {
    }
}
