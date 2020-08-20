using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaHorse : MonoBehaviour
{
    //public Transform crabForm;
    public float speed = 5;
    public float acceleration = 3;

    private Rigidbody rigidbody;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.AddSeahorse(this);
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 crab = player.transform.position;
        //float distanceBetween = Mathf.Sqrt(Mathf.Pow((transform.position.x - crab.x), 2) + Mathf.Pow((transform.position.y - crab.y), 2) + Mathf.Pow((transform.position.z - crab.z), 2));
        Vector3 dist = crab - transform.position;
        Vector3 direction = dist.normalized;
        Vector3 acc = direction * acceleration;
        
        rigidbody.AddForce(-acc);
        if (rigidbody.velocity.magnitude > speed) {
            rigidbody.velocity = rigidbody.velocity.normalized * speed;
        }
        transform.LookAt(crab);
    }
}
