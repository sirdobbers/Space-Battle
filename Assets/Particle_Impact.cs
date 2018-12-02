using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Impact : MonoBehaviour {

    ParticleSystem ps;
    ParticleSystem[] psArray;
    float duration; //seconds
    float timer = 0;

    Vector3 offsetVel;

    void Start() {
        ps = gameObject.GetComponent<ParticleSystem>();

        // Calculate how long the particle has been alive.
        duration = ps.startLifetime;
        
    }
	// Update is called once per frame
	void Update () {
        if (timer > duration / Time.deltaTime) {
            Destroy(gameObject);
        }
        timer++;
	}

    public void SetOffsetVel(Vector3 off) {
        offsetVel = off;
    }
}
