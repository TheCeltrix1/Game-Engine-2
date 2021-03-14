using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowLook : MonoBehaviour
{
    public Transform lookAt;
    public Transform waypoints;
    public float speed = 50;
    private Vector3[] _waypointArray;
    private int _currentPoint;

    private void Awake()
    {
        _waypointArray = new Vector3[waypoints.childCount];
        for (int i = 0; i < waypoints.childCount; i++)
        {
            _waypointArray[i] = waypoints.GetChild(i).transform.position;
        }
    }

    private void FixedUpdate()
    {
        MoveTowards();
        transform.LookAt(lookAt);
    }

    private void MoveTowards()
    {
        transform.position = Vector3.MoveTowards(transform.position, _waypointArray[_currentPoint], speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _waypointArray[_currentPoint]) < 2f && _currentPoint < _waypointArray.Length - 1)
        {
            _currentPoint++;
        }
    }
}
