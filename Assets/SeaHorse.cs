using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaHorse : MonoBehaviour
{
    //public Transform crabForm;
    public float speed = 5;
    public float acceleration = 3;
    public int hitPoints = 5;
    public float timeToDie;

    private Rigidbody rigidbody;
    private Player player;

    private bool alive;
    private float deathTime;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        player.AddSeahorse(this);
        rigidbody = GetComponent<Rigidbody>();
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!alive) {
            deathTime -= Time.deltaTime;
            if (deathTime <= 0f) {
                Destroy(gameObject);
            }
            return;
        }
        Vector3 crab = player.transform.position;
        //float distanceBetween = Mathf.Sqrt(Mathf.Pow((transform.position.x - crab.x), 2) + Mathf.Pow((transform.position.y - crab.y), 2) + Mathf.Pow((transform.position.z - crab.z), 2));
        Vector3 dist = crab - (transform.position + transform.up * 10f);
        Vector3 direction = dist.normalized;
        Vector3 acc = direction * acceleration;
        
        rigidbody.AddForce(-acc);
        if (rigidbody.velocity.magnitude > speed) {
            rigidbody.velocity = rigidbody.velocity.normalized * speed;
        }
        transform.LookAt(crab - Vector3.down);
    }

    public void TakeDamage(int damage) {
        hitPoints -= damage;
        if (hitPoints <= 0) {
            player.RemoveSeahorse(this);
            alive = false;
            deathTime = timeToDie;
        }
    }
}
