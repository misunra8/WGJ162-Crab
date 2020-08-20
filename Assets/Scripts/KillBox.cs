using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        SeaHorse sh = collision.gameObject.GetComponent<SeaHorse>();
        if (sh) {
            AkSoundEngine.PostEvent("SeahorseHurt", gameObject);
            sh.TakeDamage(10000);
        }
        Player p = collision.gameObject.GetComponent<Player>();
        if (p) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("End");
        }
        Pearl pearl = collision.gameObject.GetComponent<Pearl>();
        if (pearl) {
            Destroy(pearl.gameObject);
        }
    }
}
