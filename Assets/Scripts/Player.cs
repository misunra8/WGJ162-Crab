using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Make sure to report that we're using a model from
    // https://poly.google.com/view/2DgM36qZW2u
    public float speed = 2.0f;
    public float rotateSpeed = 0.5f;

    private Rigidbody rb;
    public float jumpModifier = 10;
    private uint _themeSong;
    
    public KeyCode forward, left, right, backward, jump;


    public float dashSpeed, dashTimeout, dashRechargeTime;
    public Image chargeBar;

    private float dashCatchTime, dashRecharge;
    private bool rightDash, leftDash;

    public Vector3 cameraOffset = new Vector3(0f, 0.5f, 0.5f);

    private bool onGround;
    private Camera cam;
    private Transform camLock;

    public float dangerThreshold, failurePoint;
    public Image detectionLeftMeter, detectionRightMeter;
    private List<SeaHorse> seahorses;

    // GameWon is a post game countdown timer to change the scene
    private float gameWon;

    void Start() {
        _themeSong = AkSoundEngine.PostEvent("GameTheme", gameObject);
        AkSoundEngine.SetSwitch("GamePlay", "Play", gameObject);
        AkSoundEngine.SetSwitch("Danger", "Clean", gameObject);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        

        cam = FindObjectOfType<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (Transform t in transform) {
            if (t.name == "Cam_Lock") {
                camLock = t;
                camLock.localPosition = cameraOffset;
            }
        }
        rightDash = false;
        leftDash = false;
        dashRecharge = 0f;
        dashCatchTime = 0f;
        chargeBar.type = Image.Type.Filled;
        chargeBar.fillMethod = Image.FillMethod.Horizontal;
        
        seahorses = new List<SeaHorse>();
        gameWon = -1f;
    }

    // Update is called once per frame
    void Update() {
        Vector3 newRotation = transform.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * rotateSpeed;
        if (newRotation.x > 180f) {
            newRotation.x -= 360f;
        }
        newRotation.x = Mathf.Clamp(newRotation.x, -85f, 70f);
        newRotation.z = 0f;
        transform.rotation = Quaternion.Euler(newRotation);
        cam.transform.localPosition = camLock.transform.position;
        cam.transform.localRotation = transform.localRotation;

        HandleInput();
        if (gameWon > 0) {
            gameWon -= Time.deltaTime;
            if (gameWon <= 0) {
                UnityEngine.SceneManagement.SceneManager.LoadScene("End");
            }
        } else {
            UpdateDetection();
        }
    }

    private void HandleInput() {
        // Get the Y vector as a direction to move forward
        float yAngle = Mathf.Deg2Rad * transform.eulerAngles.y;
        Vector3 direction = new Vector3(Mathf.Sin(yAngle), 0f, Mathf.Cos(yAngle));
        float yRightAngle = Mathf.Deg2Rad * (transform.eulerAngles.y + 90f);
        Vector3 rightDirection = new Vector3(Mathf.Sin(yRightAngle), 0f, Mathf.Cos(yRightAngle));
        Vector3 positionalOffset = new Vector3();
        if (Input.GetKey(forward) && !Input.GetKey(backward)) {
            positionalOffset += direction;
        } else if (Input.GetKey(backward) && !Input.GetKey(forward)) {
            positionalOffset -= direction;
        }
        if (Input.GetKey(right) && !Input.GetKey(left)) {
            positionalOffset += rightDirection;
        } else if (Input.GetKey(left) && !Input.GetKey(right)) {
            positionalOffset -= rightDirection;
        }

        if (Input.GetKey(jump) && onGround) {
            rb.AddForce(0, jumpModifier, 0, ForceMode.Impulse);
            AkSoundEngine.PostEvent("CrabJump", gameObject);
            onGround = false;
        }

        if (positionalOffset.magnitude > 0f) {
            transform.position += positionalOffset.normalized * Time.deltaTime * speed;
        }

        HandleDash(rightDirection);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Terrain") {
            onGround = true;
        }
    }

    private void HandleDash(Vector3 rightDirection) {
        float deltaTime = Time.deltaTime;
        if (dashRecharge - deltaTime > 0f) {
            // It's recharging!
            dashRecharge -= deltaTime;
            float completed = 1f - (dashRecharge / dashRechargeTime);
            chargeBar.color = Color.Lerp(Color.red, Color.green, completed);
            chargeBar.fillAmount = completed;
            return;
        } else if (dashRecharge > 0f) {
            dashRecharge = 0f;
            rightDash = false;
            leftDash = false;
        } else if (dashCatchTime > 0f) {
            dashCatchTime -= deltaTime;
        }
        Rigidbody r = GetComponent<Rigidbody>();
        if (Input.GetKeyDown(right) && !Input.GetKey(left)) {
            leftDash = false;
            if (rightDash && dashCatchTime > 0f) {
                r.velocity += rightDirection.normalized * dashSpeed;

                AkSoundEngine.PostEvent("SideDash",this.gameObject);

                rightDash = false;
                dashRecharge = dashRechargeTime;
            } else {
                rightDash = true;
                dashCatchTime = dashTimeout;
            }
        } else if (Input.GetKeyDown(left) && !Input.GetKey(right)) {
            rightDash = false;
            if (leftDash && dashCatchTime > 0f) {
                r.velocity -= rightDirection.normalized * dashSpeed;

                AkSoundEngine.PostEvent("SideDash", this.gameObject);

                leftDash = false;
                dashRecharge = dashRechargeTime;
            } else {
                leftDash = true;
                dashCatchTime = dashTimeout;
            }
        }

    }

    public void AddSeahorse(SeaHorse s) {
        seahorses.Add(s);
    }

    public void RemoveSeahorse(SeaHorse s) {
        seahorses.Remove(s);
    }

    public float CheckNearest() {
        float closest = Mathf.Infinity;
        SeaHorse nearestSeahorse;

        foreach (SeaHorse s in seahorses) {
            float dist = Vector3.Distance(transform.position, s.transform.position);
            if (dist < closest) {
                closest = dist;
                nearestSeahorse = s;
            }
        }
        return closest;
    }

    private void UpdateDetection() {
        if (seahorses.Count == 0) {
            // You won the game!
            gameWon = 2.5f;
            return;
        }
        float closest = CheckNearest();
        float detection = 0f;
        if (closest < failurePoint) {
            AkSoundEngine.StopPlayingID(_themeSong);
            UnityEngine.SceneManagement.SceneManager.LoadScene("End");
        }
        if (closest < dangerThreshold) {
            
            closest -= failurePoint;
            detection = 1f - (closest / (dangerThreshold - failurePoint));
            if (detection > 0.2f) {
                AkSoundEngine.SetSwitch("Danger", "Nearby", gameObject);
            } else {
                AkSoundEngine.SetSwitch("Danger", "Clean", gameObject);
            }
            detectionLeftMeter.color = Color.Lerp(Color.yellow, Color.red, detection);
            detectionRightMeter.color = Color.Lerp(Color.yellow, Color.red, detection);
        }
        
        detectionLeftMeter.fillAmount = detection;
        detectionRightMeter.fillAmount = detection;
    }
}
