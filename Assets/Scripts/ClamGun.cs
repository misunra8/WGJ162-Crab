using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamGun : MonoBehaviour
{
    public float clamBounceSize, clamBounceTime;
    public AnimationCurve ac;

    private bool clamBounceReturn;
    private float movingTime;
    private Transform top, bottom;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in transform) {
            if (t.name == "Top") {
                top = t;
            }
            if (t.name == "Bottom") {
                bottom = t;
            }
        }
        clamBounceReturn = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = bottom.localRotation.eulerAngles;
        movingTime -= Time.deltaTime;
        if (clamBounceReturn) {
            if (movingTime <= 0f) {
                movingTime = clamBounceTime + movingTime;
                currentRotation.x = ac.Evaluate(movingTime / clamBounceTime);
                clamBounceReturn = false;
            } else { 
                currentRotation.x = ac.Evaluate(1f - movingTime / clamBounceTime);
            }
        } else {
            if (movingTime <= 0f) {
                movingTime = clamBounceTime + movingTime;
                currentRotation.x = ac.Evaluate(1f - movingTime / clamBounceTime);
                clamBounceReturn = true;
            } else {
                currentRotation.x = ac.Evaluate(movingTime / clamBounceTime);
            }
        }
        currentRotation.x *= clamBounceSize;
        currentRotation.y = 0f;
        currentRotation.z = 0f;
        top.localRotation = Quaternion.Euler(-currentRotation);
        bottom.localRotation = Quaternion.Euler(currentRotation);
    }
}
