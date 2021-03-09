using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public Transform[] targets;
    public int currentTarget;

    private float _maxSpeed = 2.5f;
    private float _banking = 0.5f;
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
        MoveToPoint(targets[currentTarget].position);
    }

    #region Behaviours
    private Vector3 CalculateForces(Vector3 targetPos)
    {
        Vector3 relativePos = targetPos - transform.position;
        relativePos.Normalize();
        relativePos *= (_maxSpeed/2);
        //relativePos -= _rb.velocity;

        return relativePos;
    }

    private void MoveToPoint(Vector3 targetPos)
    {
        Vector3 point = CalculateForces(targetPos);
        _rb.AddForce(point);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity,_maxSpeed);
        
        Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (point * _banking), Time.deltaTime * 1.0f);
        transform.LookAt(transform.position + _rb.velocity, tempUp);

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
