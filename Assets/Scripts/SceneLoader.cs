using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string nextScene;

    public uint _event;

    // Start is called before the first frame update
    void Start() {

        string scenename = SceneManager.GetActiveScene().name;

        if (scenename =="Startup") {
            AkSoundEngine.SetSwitch("GamePlay", "Start", gameObject);
            _event = AkSoundEngine.PostEvent("GameTheme", gameObject);
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            AkSoundEngine.StopPlayingID(_event);
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }
}
