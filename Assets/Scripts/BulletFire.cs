using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    public float deathTime = 5;
    void Start()
    {
        Destroy(this.gameObject, deathTime);
    }
}
