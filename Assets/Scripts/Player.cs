using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Make sure to report that we're using a model from
    // https://poly.google.com/view/2DgM36qZW2u
    public float speed = 2.0f;
    public float rotateSpeed = 0.5f;

    public KeyCode forward, left, right, backward;

    public Vector3 cameraOffset = new Vector3(0f, 0.5f, 0.5f);

    private Camera cam;
    private Transform camLock;

    private Transform facing;

    // Start is called before the first frame update
    void Awake()
    {
        cam = FindObjectOfType<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (Transform t in transform) {
            if (t.name == "Cam_Lock") {
                camLock = t;
                camLock.localPosition = cameraOffset;
            }
            if (t.name == "Face_Direction") {
                facing = t;
            }
        }
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
    }

    private void HandleInput() {
        // Get the Y vector as a direction to move forward
        float yAngle = Mathf.Deg2Rad * transform.eulerAngles.y;
        Vector3 direction = new Vector3(Mathf.Sin(yAngle), 0f, Mathf.Cos(yAngle));
        float yRightAngle = Mathf.Deg2Rad * (transform.eulerAngles.y + 90f);
        Vector3 rightDirection = new Vector3(Mathf.Sin(yRightAngle), 0f, Mathf.Cos(yRightAngle));
        Debug.Log("Y rotation = " + yAngle +  " | x: " + direction.x + ", z: " + direction.z);
        facing.position = transform.position + direction * 4f;
        if (Input.GetKey(forward) && !Input.GetKey(backward)) {
            transform.position += direction * Time.deltaTime * speed;
        } else if (Input.GetKey(backward) && !Input.GetKey(forward)) {
            transform.position -= direction * Time.deltaTime * speed;
        } else if (Input.GetKey(right) && !Input.GetKey(left)) {
            transform.position += rightDirection * Time.deltaTime * speed;
        } else if (Input.GetKey(left) && !Input.GetKey(right)) {
            transform.position -= rightDirection * Time.deltaTime * speed;
        }
    }
}
