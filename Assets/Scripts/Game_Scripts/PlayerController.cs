using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Speed")]
    public float movingSpeed = 15f;


    [Header("Jump")]
    public float jumpForce = 100f;
    public bool isGrounded = true;


    [Header("Turn Around")]
    public float turningDuration = 0.2f;
    public bool isTurning = false;

    private Quaternion turnStart;
    private Quaternion turnEnd;
    private float turningRate;
    private float turningTime = 0;


    [Header("Position")]
    public float roadWidth;
    public bool isOnLeftRoad = false;
    public bool isOnMiddleRoad = true;
    public bool isOnRightRoad = false;

    public float movingDuration = 0.5f;
    public bool isSwitching = false;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float movingRate;
    private float movingTime = 0;

    
    private Animator playerAnim;

    private void Start() 
    {
        playerAnim = gameObject.GetComponent<Animator>();

        turningRate = 1 / turningDuration;
        movingRate = 1 / movingDuration;
    }

    private void Update() 
    {
        // turning around
        if (isTurning) {
            turningTime += turningRate * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(turnStart, turnEnd, turningTime);

            if(turningTime > 1.0f) {
                transform.rotation = Quaternion.Slerp(turnStart, turnEnd, 1);
                isTurning = false;
            }
        }
        else turningTime = 0;

        // switching road
        if (isSwitching) {
            movingTime += movingRate * Time.deltaTime;
            transform.position = Vector3.Lerp(moveStart, moveEnd, movingTime);

            if(movingTime > 1.0f) {
                transform.position = Vector3.Lerp(moveStart, moveEnd, 1);
                isSwitching = false;
            }
        }
        else movingTime = 0;

        // moving forward in a constant speed
        if(!isSwitching)
            transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);            

        // move to left road or turning left
        if(Input.GetKeyDown(KeyCode.A) && !isTurning && !isSwitching) {
            if(isOnLeftRoad) {
                SetRotateStatus(true);
                isTurning = true;
            }
            else {
                MovingToNextRoad("Left");
                isSwitching = true;
            }
        }
        // move to right road or turning right
        else if(Input.GetKeyDown(KeyCode.D) && !isTurning && !isSwitching) {
            if(isOnRightRoad) {
                SetRotateStatus(false);
                isTurning = true;
            }
            else {
                MovingToNextRoad("Right");
                isSwitching = true;
            }
        }
        // jumping
        else if(Input.GetKeyDown(KeyCode.W) && isGrounded) {
            gameObject.GetComponent<Rigidbody>().AddForce(jumpForce * Vector3.up);
            isGrounded = false;

            playerAnim.SetBool("isGrounded", false);
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.collider.CompareTag("Ground")) {
            isGrounded = true;
            
            playerAnim.SetBool("isGrounded", true);
        }
    }

    void SetRotateStatus(bool isTurningLeft) 
    {
        Vector3 degree;
        if (isTurningLeft) degree = new Vector3(0, -90,  0);
        else degree = new Vector3(0, 90,  0);

        turnStart = transform.rotation;
        turnEnd = turnStart * Quaternion.Euler(degree);
    }

    void SetSwitchStatus(bool isMovingLeft) 
    {
        Vector3 dir;
        if (isMovingLeft) 
            dir = Vector3.left * roadWidth + Vector3.forward * (movingSpeed * movingDuration);
        else 
            dir = Vector3.right * roadWidth + Vector3.forward * (movingSpeed * movingDuration);

        moveStart = transform.position;
        moveEnd = moveStart + dir;
    }

    void SetPosition(string name) 
    {
        if(name == "Left") {
            isOnLeftRoad = true;
            isOnMiddleRoad = false;
            isOnRightRoad = false;
        }
        else if(name == "Middle") {
            isOnLeftRoad = false;
            isOnMiddleRoad = true;
            isOnRightRoad = false;
        }
        else if(name == "Right") {
            isOnLeftRoad = false;
            isOnMiddleRoad = false;
            isOnRightRoad = true;
        }
    }

    void MovingToNextRoad(string dir) 
    {
        if(dir == "Left") {
            if (isOnRightRoad) SetPosition("Middle");
            else if (isOnMiddleRoad) SetPosition("Left");

            SetSwitchStatus(true);
        }
        else if(dir == "Right") {
            if (isOnLeftRoad) SetPosition("Middle");
            else if (isOnMiddleRoad) SetPosition("Right");

            SetSwitchStatus(false);
        }
    }
}
