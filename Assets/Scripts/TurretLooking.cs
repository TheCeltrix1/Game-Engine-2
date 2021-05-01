using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLooking : MonoBehaviour
{
    public Transform target;
    public float clampDegrees;
    public Transform[] barrelLocations;
    public GameObject bulletPewPew;
    private float _bulletSpeed = 40;
    private float _fireRate = 0.1f;
    private float _fire;
    private int _fireLocation;

    private void Awake()
    {
        _fire = Random.Range(0f, 1f);
    }

    void FixedUpdate()
    {
        Quaternion face = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position, Vector3.up), 10);
        transform.rotation = face;
        if (_fire >= _fireRate)
        {
            GameObject obj = Instantiate(bulletPewPew, barrelLocations[_fireLocation].position, transform.rotation);
            obj.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;
            _fire = 0;
            _fireLocation++;
            if (_fireLocation >= barrelLocations.Length) _fireLocation = 0;
        }
        else _fire += Time.deltaTime;
    }
}
