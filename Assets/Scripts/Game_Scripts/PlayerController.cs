using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movingSpeed;


    private void Update() {
        if(Input.GetKeyDown(KeyCode.A)) {
            Vector3 degree = new Vector3(0, 90, 0);
            Vector3 newRot = transform.rotation.eulerAngles - degree;

            transform.rotation = Quaternion.Euler(newRot);
        }
        else if(Input.GetKeyDown(KeyCode.D)) {
            Vector3 degree = new Vector3(0, 90, 0);
            Vector3 newRot = transform.rotation.eulerAngles + degree;

            transform.rotation = Quaternion.Euler(newRot);
        }
        else {
            transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);            
        }
    }
}
