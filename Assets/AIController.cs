using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float pointDistance = 5;

    public GameObject _targetObj;
    private float _bypassAngle;
    private Vector3 _targetLocation;
    private Rigidbody _rb;
    private int _huntState;
    private Rigidbody _targetRb;
    private float _forwardTracking = 0.5f;

    void Awake()
    {
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.useGravity = false;
        FindTarget();
        _bypassAngle = Random.Range(0, 360);
    }

    void FindTarget()
    {
        //_targetObj = GameManager.NearestPlayer(this.gameObject);
        _targetRb = _targetObj.GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.AddForce(CalculateForces(_targetObj.transform.position + BypassTarget()));
        transform.LookAt(transform.position + _rb.velocity);
    }

    Vector3 CalculateForces(Vector3 targetPos)
    {
        Vector3 force = Vector3.zero;

        Vector3 relativePos = targetPos - transform.position;
        relativePos.Normalize();
        relativePos *= maxSpeed;
        relativePos -= _rb.velocity;

        return relativePos;
    }

    /*Vector3 PursueTarget()
    {

    }*/

    Vector3 BypassTarget()
    {
        Vector3 relativeAngle = Vector3.zero;
        relativeAngle += transform.right * Mathf.Sin(_bypassAngle) * pointDistance;
        relativeAngle += transform.up * Mathf.Cos(_bypassAngle) * pointDistance;
        return relativeAngle;
    }

    void ShootTarget()
    {
        Debug.Log("Pew Pew!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_targetObj.transform.position + BypassTarget(), Vector3.one);
    }
}
