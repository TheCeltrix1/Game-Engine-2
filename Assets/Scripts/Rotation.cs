using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationPerSecond;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (rotationPerSecond * Time.deltaTime));
    }
}
