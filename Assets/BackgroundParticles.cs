using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour {

    GameObject cameraObject;
    public ParticleSystem ps;

    void Start() {
        cameraObject = Camera.main.gameObject;
    }
    
    void Update () {
        float oSize = Camera.main.orthographicSize*0.12f;
        ParticleSystem.ShapeModule Shape = ps.shape;
        Shape.scale = new Vector3(oSize, oSize, oSize)*50;
        ps.maxParticles = (int)(oSize*15)+15;
        ParticleSystem.EmissionModule Emission = ps.emission;
        Emission.rate = (int)(oSize * 15) + 15;
        


        /*
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        int numParticlesAlive = ps.GetParticles(particles);
        for (int i = 0; i < numParticlesAlive; i++) {
            float Dist = Vector3.Distance(particles[i].position, camera.transform.position);
            print(Dist);
            if (Dist > 25f) {
                particles[i].remainingLifetime = 0;
            }
            ps.SetParticles(particles, numParticlesAlive);
        }
        */
    }
}
