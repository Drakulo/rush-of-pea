using UnityEngine;
using System.Collections;

public class CarrotTrigger : MonoBehaviour
{
    public AudioClip Clip;

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(Clip, Vector3.zero);
        }
    }
}
