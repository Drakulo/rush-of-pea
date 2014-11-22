using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour
{
    public float RotationSpeed;
    public float OscillationSpeed;
    public float OscillationAmplitude;

    private float startY;

    void Start()
    {
        startY = transform.position.y;

        // random start rotation
        transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360));
    }

    void Update()
    {
        // wave
        var pos = transform.position;
        pos.y = startY + Mathf.Sin(Time.time * OscillationSpeed) * OscillationAmplitude;
        transform.position = pos;

        // rotation
        transform.Rotate(new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
    }
}
