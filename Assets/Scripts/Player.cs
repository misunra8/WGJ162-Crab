using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotateSpeed = 0.5f;

    public KeyCode forward, left, right, backwards;

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
        transform.rotation = Quaternion.Euler(newRotation);
        cam.transform.localPosition = camLock.transform.position;
        cam.transform.localRotation = transform.localRotation;

        HandleInput();
    }

    private void HandleInput() {
        // Get the Y vector as a direction to move forward
        float yAngle = Mathf.Deg2Rad * transform.eulerAngles.y;
        Vector3 direction = new Vector3(Mathf.Sin(yAngle), 0f, Mathf.Cos(yAngle));
        Debug.Log("Y rotation = " + yAngle +  " | x: " + direction.x + ", z: " + direction.z);
        facing.position = transform.position + direction * 4f;
        if (Input.GetKey(forward) && !Input.GetKey(backwards)) {
            transform.position += direction * Time.deltaTime * speed;
        }
    }
}
