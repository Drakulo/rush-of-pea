using UnityEngine;
using System.Collections;

public class LooseCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        LevelGenerator.Loose();
    }
}
