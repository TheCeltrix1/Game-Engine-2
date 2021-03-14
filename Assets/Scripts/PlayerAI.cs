using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public Transform targetHolder;
    private Vector3[] _targets;
    public int currentTarget;

    private float _maxSpeed = 5f;
    private float _banking = 0.1f;
    private Rigidbody _rb;

    private void Awake()
    {
        _targets = new Vector3[targetHolder.childCount];
        for (int i = 0; i < targetHolder.childCount; i++)
        {
            _targets[i] = targetHolder.GetChild(i).transform.position;
        }
        if (GetComponent<Rigidbody>()) _rb = GetComponent<Rigidbody>();
        else
        {
            _rb = gameObject.AddComponent<Rigidbody>();
            _rb.useGravity = false;
        }
    }

    private void Update()
    {
        MoveToPoint(_targets[currentTarget]);
    }

    #region Behaviours
    private Vector3 CalculateForces(Vector3 targetPos)
    {
        Vector3 relativePos = targetPos - transform.position;
        relativePos.Normalize();
        relativePos *= (_maxSpeed/2);

        return relativePos;
    }

    private void MoveToPoint(Vector3 targetPos)
    {
        Vector3 point = CalculateForces(targetPos);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity + point, _maxSpeed);
        
        Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (targetPos - transform.position), Time.deltaTime * 10.0f);
        transform.LookAt(transform.position + _rb.velocity, tempUp);

        if (Vector3.Distance(this.transform.position, _targets[currentTarget]) <= 2f)
        {
            if (currentTarget < _targets.Length - 1)
            {
                currentTarget++;
            }
        }
    }
    #endregion
}
