using UnityEngine;
using System.Collections;

public class CarrotRotation : MonoBehaviour
{
    public float RotationSpeed;

    void Start()
    {
        // random start rotation
        transform.Rotate(new Vector3(0, 1, 0), Random.Range(0, 360));
    }

    void Update()
    {

        // rotation
        transform.Rotate(new Vector3(0, 1, 0), RotationSpeed * Time.deltaTime);
    }
}
