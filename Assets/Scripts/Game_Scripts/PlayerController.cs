using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move Speed")]
    public float movingSpeed = 15f;
    public float movingUp = 4f;
    private float cnt = 0;


    [Header("Jump")]
    public float jumpForce = 100f;
    public bool isGrounded = true;


    [Header("Turn a round")]
    public float turningDuration = 0.2f;
    public bool isTurning = false;
    public float inputCoolDown = 0.5f;
    public bool isCoolDown = false;

    private Quaternion turnStart;
    private Quaternion turnEnd;
    private float turningRate;
    private float turningTime = 0;


    [Header("Position")]
    public float roadWidth;
    public bool isOnLeftRoad = false;
    public bool isOnMiddleRoad = true;
    public bool isOnRightRoad = false;
    public bool isInTurningArea = false;

    public float movingDuration = 0.5f;
    public bool isSwitching = false;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private float movingRate;
    private float movingTime = 0;

    public bool isDead = false;
    
    public AudioManager audioManager;
    public GameManager gameManager;

    private Animator playerAnim;

    private Collider turningArea;

    private void Start() 
    {
        playerAnim = gameObject.GetComponent<Animator>();

        turningRate = 1 / turningDuration;
        movingRate = 1 / movingDuration;
    }

    private void Update() 
    {
        // turning around
        if (isTurning && !isDead) {
            turningTime += turningRate * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(turnStart, turnEnd, turningTime);

            if(turningTime > 1.0f) {
                transform.rotation = Quaternion.Slerp(turnStart, turnEnd, 1);
                isTurning = false;
                Invoke("CoolDownEnd", inputCoolDown);
            }
        }
        else turningTime = 0;

        // switching road
        if (isSwitching && !isDead) {
            movingTime += movingRate * Time.deltaTime;
            transform.position = Vector3.Lerp(moveStart, moveEnd, movingTime);

            if(movingTime > 1.0f) {
                transform.position = Vector3.Lerp(moveStart, moveEnd, 1);
                isSwitching = false;
            }
        }
        else movingTime = 0;

        if (GameManager.survivalTime / 10 >= cnt) {
            movingSpeed += movingUp;
            cnt++;
        }
        // moving forward in a constant speed
        if(!isSwitching && !isTurning && !isDead)
            transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);            
            //transform.Translate(transform.forward * movingSpeed * Time.deltaTime);

        // move to left road or turning left
        if(Input.GetKeyDown(KeyCode.A) && !isTurning && !isSwitching && !isCoolDown && !isDead) {
            if(isOnLeftRoad && isInTurningArea) {
                SetRotatePos();
                SetRotateStatus(true);
                isTurning = true;
                isCoolDown = true;
            }
            else if(isOnLeftRoad){
                Debug.Log("can't left");
            }
            else {
                MovingToNextRoad("Left");
                isSwitching = true;
            }
        }
        // move to right road or turning right
        else if(Input.GetKeyDown(KeyCode.D) && !isTurning && !isSwitching && !isCoolDown && !isDead) {
            if(isOnRightRoad && isInTurningArea) {
                SetRotatePos();
                SetRotateStatus(false);
                isTurning = true;
                isCoolDown = true;
            }
            else if(isOnRightRoad) {
                Debug.Log("cant't right");
            }
            else {
                MovingToNextRoad("Right");
                isSwitching = true;
            }
        }
        // jumping
        else if(Input.GetKeyDown(KeyCode.W) && isGrounded && !isDead) {
            gameObject.GetComponent<Rigidbody>().AddForce(jumpForce * Vector3.up);
            isGrounded = false;

            audioManager.Play("Jump");
            playerAnim.SetBool("isGrounded", false);
        }
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.collider.CompareTag("Ground")) {
            isGrounded = true;
            
            playerAnim.SetBool("isGrounded", true);
        }

        if(collision.collider.CompareTag("Obstacle")) {
            isDead = true;
            playerAnim.SetBool("isDead", true);
            Catch();
            Invoke("Gameover", 3);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("TurningArea")) {
            isInTurningArea = true;
            turningArea = other;
        }
        if(other.CompareTag("coin")) {
            GameManager.coinN += 1;
            audioManager.Stop("CoinSound");
            audioManager.Play("CoinSound");
            Destroy(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("TurningArea")) {
            isInTurningArea = false;
            turningArea = null;
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
            //dir = Vector3.left * roadWidth + Vector3.forward * (movingSpeed * movingDuration);
            dir = -transform.right * roadWidth + transform.forward * (movingSpeed * movingDuration);
        else
            //dir = Vector3.right * roadWidth + Vector3.forward * (movingSpeed * movingDuration);
            dir = transform.right * roadWidth + transform.forward * (movingSpeed * movingDuration);

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

    void SetRotatePos() 
    {
        Vector3 middlePos = turningArea.transform.Find("middlePos").position;
        Vector3 leftPos = turningArea.transform.Find("leftPos").position;
        Vector3 rightPos = turningArea.transform.Find("rightPos").position;
        Vector3 pos = Vector3.zero;

        float leftDis = (leftPos - transform.position).magnitude;
        float middleDis = (middlePos - transform.position).magnitude;
        float rightDis = (rightPos - transform.position).magnitude;

        if (middleDis <= leftDis && middleDis <= rightDis) {
            pos = middlePos;
            SetPosition("Middle");
        }
        else if (rightDis <= leftDis && rightDis <= middleDis) {
            pos = rightPos;
            SetPosition("Right");
        }
        else if (leftDis <= rightDis && leftDis <= middleDis) {
            pos = leftPos;
            SetPosition("Left");
        }
        
        Vector3 newPos = new Vector3(pos.x, transform.position.y, pos.z);

        transform.position = newPos;
    }

    void CoolDownEnd() 
    {
        isCoolDown = false;
    }

    void Catch() 
    {
        GameObject Ghoul = GameObject.Find("Player/Ghoul");

        Ghoul.transform.position = transform.position - transform.forward * 3;
        Ghoul.GetComponent<Animation>().Stop();
        Ghoul.GetComponent<Animation>().Play("Attack2");
        Ghoul.GetComponent<Animation>().PlayQueued("Idle", QueueMode.CompleteOthers);

        audioManager.Play("Zombie");
    }

    void Gameover() {
        Time.timeScale = 0;
        gameManager.gameover.SetActive(true);

        audioManager.Stop("GameBgm");
        audioManager.Play("Gameover");

        gameManager.endSurvivalText.text = "Score : " + ((int)GameManager.survivalTime).ToString();
        gameManager.endCoinText.text = "Coin : " + (GameManager.coinN).ToString();
    }
}
