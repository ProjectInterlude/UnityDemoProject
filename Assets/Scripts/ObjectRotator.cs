using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public Transform objectToRotate;
    public float rotationSpeed;
    public float verticalPeriod, verticalAmplitude;
    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = objectToRotate.position;
    }
    // Update is called once per frame
    void Update()
    {
        objectToRotate.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        if(verticalPeriod > 0)
        {
            objectToRotate.position = initialPosition + Mathf.Cos(Time.time * 2 * Mathf.PI / verticalPeriod) * verticalAmplitude * Vector3.up;
        }
    }
}
