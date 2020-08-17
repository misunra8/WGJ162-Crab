using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaHorse : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 crab = GameObject.Find("Geo_Crab").transform.position;
        float distanceBetween = Mathf.Sqrt(Mathf.Pow((transform.position.x - crab.x), 2) + Mathf.Pow((transform.position.y - crab.y), 2) + Mathf.Pow((transform.position.z - crab.z), 2));
    }
}
