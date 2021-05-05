using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float maxSpeed = 5;
    public float pointDistance = 5;
    public bool fixedTargets;

    public AudioSource engines;
    public AudioSource bulletFiring;
    public GameObject targetObj;
    public Rigidbody rb;
    public GameObject bulletPewPew;
    public float fireRate = 0.5f;

    private float _randomFireOffset;
    private bool _canFire;
    private float _fireAngle = 1f;
    private float _bulletSpeed = 40;
    private float _bypassAngle;
    //private float _forwardTracking = 5f;
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

        bulletFiring.loop = false;
        engines.loop = true;
        engines.Play();

        #region SphereTrigger
        if (!GetComponent<SphereCollider>()) gameObject.AddComponent<SphereCollider>();
        _areaTrigger = GetComponent<SphereCollider>();
        _areaTrigger.isTrigger = true;
        //_areaTrigger.radius = _areaTrigger.radius * transform.localScale.x;
        _randomFireOffset = Random.Range(0,fireRate);
        #endregion
    }

    private void Start()
    {
        if (!fixedTargets || targetObj == null) targetObj = GameManager.NearestPlayer(this.gameObject);
        StartCoroutine(FireTimer());
    }

    void FixedUpdate()
    {
        _targetTransform = targetObj.transform;
        if (_avoid) FlyByBehaviour(avoidObject);
        else PursueTargetBehaviour();
        Bank(_targetTransform.position);
    }

    public Vector3 CalculateForces(Vector3 targetPos)
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
        if (Vector3.Angle(rb.velocity.normalized, transform.forward) <= _fireAngle && _canFire)
        {
            ShootTarget();
            _canFire = false;
        }
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
        float turnAngle = Vector3.Angle(transform.forward, rb.velocity * 100);
        transform.Rotate(new Vector3(Mathf.Lerp(0, turnAngle, 0.75f), 0, 0));
    }

    private void Bank(Vector3 currentPoint)
    {
        Vector3 tempUp = Vector3.Lerp(Vector3.up, (Vector3.up + currentPoint).normalized, .75f);
        transform.LookAt(transform.position + rb.velocity, tempUp);
    }

    void ShootTarget()
    {
        bulletFiring.Play();
        GameObject obj = Instantiate(bulletPewPew, transform.position, transform.rotation);
        obj.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;
    }
    #endregion

    IEnumerator FireTimer()
    {
        while (true) 
        {
            yield return new WaitForSeconds(fireRate + _randomFireOffset);
            _canFire = true;
            _randomFireOffset = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<BulletFire>())
        {
            _avoid = true;
            avoidObject = other.gameObject;
            if (other.GetComponent<AIController>() && other.GetComponent<AIController>().avoidObject == this.gameObject) _avoid = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<BulletFire>()) _avoid = false;
        //_avoidObject = _targetObj;
    }
}
