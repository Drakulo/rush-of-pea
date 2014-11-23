using UnityEngine;
using System.Collections;

public class BigPea : MonoBehaviour
{
    public AudioClip RoarSound;

    public float MinWaitTime = 2F;
    public int SoundGap;

    private bool wait = false;

    void Update()
    {
        if (!wait)
        {
            var rand = Random.Range(0, 100);
            if (rand < SoundGap)
            {
                AudioSource.PlayClipAtPoint(RoarSound, Vector3.zero);
                wait = true;
                StartCoroutine(WaitForNextSound());
            }
        }
    }

    private IEnumerator WaitForNextSound()
    {
        yield return new WaitForSeconds(MinWaitTime);
        wait = false;
    }
}
