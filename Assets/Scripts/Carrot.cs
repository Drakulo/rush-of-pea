using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour
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
        // wave
        var pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * OscillationSpeed) * OscillationAmplitude;
        transform.position = pos;
    }
}
