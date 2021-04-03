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
    public Rigidbody rb;
    private float _forwardTracking = 5f;

    void Awake()
    {
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        _targetTransform = _targetObj.transform;
        FindTarget();
        BypassTargetGeneration();
        GetComponent<ObstacleAvoidance>().enabled = true;
    }

    void FindTarget()
    {
        //_targetObj = GameManager.NearestPlayer(this.gameObject);
        //_targetRb = _targetObj.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        /*if (Vector3.Angle(_targetRb.velocity, _rb.velocity) >= 100)
        {
            FlyByBehaviour();
        }*/
        
        FlyByBehaviour();
        Banking();
    }

    public virtual Vector3 CalculateForces(Vector3 targetPos)
    {
        Vector3 relativePos = targetPos - transform.position;
        relativePos.Normalize();
        relativePos *= maxSpeed;
        relativePos -= rb.velocity;

        return relativePos;
    }

    #region Behaviours
    void PursueTargetBehaviour()
    {
        rb.AddForce(CalculateForces(_targetTransform.position + (_targetTransform.forward * _forwardTracking)));
    }

    void FlyByBehaviour()
    {
        if (Vector3.Distance(transform.position, _targetTransform.position + _targetLocation) <= 0.5f) BypassTargetGeneration();
        rb.AddForce(CalculateForces(_targetTransform.position + _targetLocation));
    }

    void Avoidance()
    {

    }

    void BypassTargetGeneration()
    {
        _bypassAngle = Random.Range(0, 360);
        Vector3 relativeAngle = Vector3.zero;
        relativeAngle += transform.right * Mathf.Sin(_bypassAngle) * pointDistance;
        relativeAngle += transform.up * Mathf.Cos(_bypassAngle) * pointDistance;
        _targetLocation = relativeAngle;
    }

    void Banking()
    {
        //Vector3 bankingValue = (_targetLocation - _rb.velocity).normalized + (Vector3.up * 2);
        transform.LookAt(transform.position + rb.velocity);
        float turnAngle = Vector3.Angle(transform.forward,rb.velocity);
        transform.Rotate(new Vector3(Mathf.Lerp(0,turnAngle,0.75f),0,0));
    }

    void ShootTarget()
    {
        Debug.Log("Pew Pew!");
    }
    #endregion
}
