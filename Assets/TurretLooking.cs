using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLooking : MonoBehaviour
{
    public Transform target;
    public float clampDegrees;

    void FixedUpdate()
    {
        Quaternion face = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position, Vector3.up), 10);
        //face.eulerAngles = new Vector3(Mathf.Clamp(face.eulerAngles.x, -clampDegrees, clampDegrees), face.eulerAngles.y, face.eulerAngles.z);
        Debug.Log(face.eulerAngles.x);
        transform.rotation = face;
    }
}
