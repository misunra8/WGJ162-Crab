using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamGun : MonoBehaviour
{
    public float reloadTime, fireSpeed;

    private float reloading;

    public Pearl weapon;

    public float clamBounceSize, clamBounceTime;
    public AnimationCurve ac;

    private bool clamBounceReturn;
    private float movingTime;
    private Transform top, bottom;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
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

        reloading -= Time.deltaTime;
        if (reloading < 0f) {
            if (Input.GetMouseButton(0)) {
                reloading = reloadTime;
                Pearl pearl = Instantiate(weapon);
                pearl.transform.localPosition = transform.position;
                pearl.GetComponent<Rigidbody>().velocity = player.transform.forward * fireSpeed;
            }
        }
    }
}
