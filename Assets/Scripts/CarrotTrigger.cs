using UnityEngine;
using System.Collections;

public class CarrotTrigger : MonoBehaviour
{
    public AudioClip Clip;

    void OnTriggerEnter(Collider c)
    {
        AudioSource.PlayClipAtPoint(Clip, Vector3.zero);
    }
}
