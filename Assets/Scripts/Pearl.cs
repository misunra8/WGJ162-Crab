using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pearl : MonoBehaviour
{
    public float surviveTime = 40f;

    private float aliveTime;
    // Start is called before the first frame update
    void Start()
    {
        aliveTime = surviveTime;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime < 0f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        SeaHorse sh = collision.gameObject.GetComponent<SeaHorse>();
        if (sh) {
            AkSoundEngine.PostEvent("SeahorseHurt", gameObject);
            sh.TakeDamage(1);
        }
    }
}
