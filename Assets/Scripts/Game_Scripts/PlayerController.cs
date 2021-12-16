using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movingSpeed = 15f;

    public float duration = 0.2f;

    public float jumpForce = 100f;
    private bool isGrounded = true;

    private bool isTuring = false;
    private Quaternion start;
    private Quaternion end;
    private float rate;
    private float time = 0;

    private void Start() {
        rate = 1 / duration;
    }
    private void Update() {
        if (isTuring) {
            time += rate * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, end, time);

            if(time > 1.0f) {
                transform.rotation = Quaternion.Slerp(start, end, 1);
                isTuring = false;
            }
        }
        else time = 0;

        transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);            

        if(Input.GetKeyDown(KeyCode.A)) {
            SetRotateStatus(true);
            isTuring = true;
        }
        else if(Input.GetKeyDown(KeyCode.D)) {
            SetRotateStatus(false);
            isTuring = true;
        }
        else if(Input.GetKeyDown(KeyCode.W) && isGrounded) {
            gameObject.GetComponent<Rigidbody>().AddForce(jumpForce * Vector3.up);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    void SetRotateStatus(bool isTuringLeft) {
        Vector3 degree;
        if (isTuringLeft) degree = new Vector3(0, -90,  0);
        else degree = new Vector3(0, 90,  0);

        start = transform.rotation;
        end = start * Quaternion.Euler(degree);
    }
}
