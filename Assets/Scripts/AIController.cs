﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float pointDistance = 5;

    public GameObject _targetObj;
    public Rigidbody rb;

    private float _bypassAngle;
    private float _forwardTracking = 5f;
    private Vector3 _targetLocation;
    private Transform _targetTransform;
    private SphereCollider _areaTrigger;
    private bool _avoid;

    [HideInInspector] public GameObject avoidObject;

    void Awake()
    {
        if (!GetComponent<Rigidbody>()) gameObject.AddComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;

        FindTarget();
        //GetComponent<ObstacleAvoidance>().enabled = true;

        #region SphereTrigger
        if (!GetComponent<SphereCollider>()) gameObject.AddComponent<SphereCollider>();
        _areaTrigger = GetComponent<SphereCollider>();
        _areaTrigger.isTrigger = true;
        //_areaTrigger.radius = _areaTrigger.radius * transform.localScale.x;
        #endregion
    }

    void FindTarget()
    {
        //_targetObj = GameManager.NearestPlayer(this.gameObject);
        //_targetRb = _targetObj.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _targetTransform = _targetObj.transform;
        if (_avoid) FlyByBehaviour(avoidObject);
        else PursueTargetBehaviour();
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
        rb.AddForce(CalculateForces(_targetTransform.position /*+ (_targetTransform.forward * _forwardTracking)*/));
    }

    void FlyByBehaviour(GameObject avoidObject)
    {
        if (Vector3.Distance(transform.position, avoidObject.transform.position + _targetLocation) <= 0.5f) BypassTargetGeneration();
        rb.AddForce(CalculateForces(avoidObject.transform.position + _targetLocation));
    }

    private void BypassTargetGeneration()
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

    private void OnTriggerStay(Collider other)
    {
        _avoid = true;
        avoidObject = other.gameObject;
        if (other.GetComponent<AIController>() && other.GetComponent<AIController>().avoidObject == this.gameObject) _avoid = false;
    }
    private void OnTriggerExit(Collider other)
    {
        _avoid = false;
        //_avoidObject = _targetObj;
    }
}
