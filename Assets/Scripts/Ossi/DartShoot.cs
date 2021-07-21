using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float triggerRange;
    public float fireRate;
    [SerializeField]
    Vector3 rayDirection = Vector3.left;

    private float nextFire = 0f;

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, triggerRange) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            shoot();
            //Debug.Log(hit.transform.name);
        }
    }

    void shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, -90, 0));
    }
}
