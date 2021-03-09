using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public Transform[] targets;
    public int currentTarget;

    private float _maxSpeed = 2.5f;
    private Rigidbody _rb;

    private void Awake()
    {
        if (GetComponent<Rigidbody>()) _rb = GetComponent<Rigidbody>();
        else
        {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.useGravity = false;
            //_rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position - _rb.velocity);
        MoveToPoint();
    }

    #region Behaviours
    private Vector3 CalculateForces(Vector3 targetPos)
    {
        Vector3 force = Vector3.zero;

        Vector3 relativePos = targetPos - transform.position;
        relativePos.Normalize();
        relativePos *= _maxSpeed;
        relativePos -= _rb.velocity;

        return relativePos;
    }

    private void MoveToPoint()
    {
        _rb.AddForce(CalculateForces(targets[currentTarget].position));
        if (Vector3.Distance(this.transform.position, targets[currentTarget].position) <= .5f)
        {
            if (currentTarget < targets.Length - 1)
            {
                currentTarget++;
            }
            else Debug.Log("Targets Reached");
        }
    }
    #endregion
}
