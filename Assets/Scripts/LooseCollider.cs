using UnityEngine;
using System.Collections;

public class LooseCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            LevelGenerator.Loose();
        }
    }
}
