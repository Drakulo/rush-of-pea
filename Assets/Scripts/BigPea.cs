using UnityEngine;
using System.Collections;

public class BigPea : MonoBehaviour
{
    public float OscillationSpeed;
    public float OscillationAmplitude;

    private float startY;
    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        var pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * OscillationSpeed) * OscillationAmplitude;
        transform.position = pos;
    }
}
