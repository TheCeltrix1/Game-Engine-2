using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float pointDistance = 5;

    public GameObject _targetObj;
    private Transform _targetTransform;
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
        _targetTransform = _targetObj.transform;
        FindTarget();
        BypassTargetGeneration();
    }

    void FindTarget()
    {
        //_targetObj = GameManager.NearestPlayer(this.gameObject);
        _targetRb = _targetObj.GetComponent<Rigidbody>();
    }

    void Update()
    {
        /*if (Vector3.Angle(_targetRb.velocity, _rb.velocity) >= 100)
        {
            BypassTargetBehaviour();
        }*/
        BypassTargetBehaviour();

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

    #region Behaviours
    void PursueTargetBehaviour()
    {
        _rb.AddForce(CalculateForces(_targetTransform.position + (_targetTransform.forward * 5)));
    }

    void BypassTargetBehaviour()
    {
        if (Vector3.Distance(transform.position, _targetTransform.position + _targetLocation) <= 0.5f) BypassTargetGeneration();
        _rb.AddForce(CalculateForces(_targetTransform.position +_targetLocation));
    }

    void BypassTargetGeneration()
    {
        _bypassAngle = Random.Range(0, 360);
        Vector3 relativeAngle = Vector3.zero;
        relativeAngle += transform.right * Mathf.Sin(_bypassAngle) * pointDistance;
        relativeAngle += transform.up * Mathf.Cos(_bypassAngle) * pointDistance;
        _targetLocation = relativeAngle;
        Debug.Log(_targetLocation);
    }

    void ShootTarget()
    {
        Debug.Log("Pew Pew!");
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_targetTransform.position + _targetLocation, Vector3.one);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(_targetTransform.position, Vector3.one);
    }
}
