using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    
    //float baseSpeed = 1.5f; //used for lerp

    public GameObject target;
    public bool debug;

    Vector3 vel = new Vector3(0, 0, 0);
    Vector3 forw = new Vector3(0, 0, 0);
    Vector3 offPos = new Vector3(0,0,0);

    float fov;
    float minFov = 2f;
    float maxFov = 80f;

    void Start() {
        fov = 5f;
    }

    // Update is called once per frame
    void Update() {
        if (target != null) {
            // get forward vec
            Quaternion QRot = target.transform.rotation;
            QRot = Quaternion.Euler(0, 0, QRot.eulerAngles.z);

            // new rel cam pos
            vel = target.GetComponent<Ship_Movement>().GetVel() * 6.5f;
            forw = QRot * new Vector3(0, 1, 0);
            //forw2 = QRot * new Vector3(0, 1, 0) * Mathf.Pow(vel.magnitude, 2);
            offPos = vel + forw * vel.magnitude * 0.1f;

            Vector3 newPos = target.transform.position + new Vector3(0, 0, -10);
            //transform.position = Vector3.Lerp(transform.position, newPos + offPos, (Mathf.Pow(vel.magnitude, 5)+baseSpeed) * Time.deltaTime);
            transform.position = newPos + offPos;
        }

        if (Input.GetMouseButton(2)) {
            fov += 0.005f;
        }


        fov -= Input.GetAxis("Mouse ScrollWheel") * 10f;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.orthographicSize = fov;
        //Camera.main.fieldOfView = fov;



    }

    void OnDrawGizmos() {
        if (target != null & debug == true) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(target.transform.position + vel + forw, 0.2f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(target.transform.position + forw, 0.2f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.transform.position + vel, 0.2f);
        }
    }
    
}
