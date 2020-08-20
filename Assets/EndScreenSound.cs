using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetSwitch("GamePlay", "End", gameObject);
        AkSoundEngine.PostEvent("GameTheme", gameObject);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
