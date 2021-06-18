using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float triggerRange;
    public float fireRate;

    private float nextFire = 0f;

    void FixedUpdate()
    {
    	RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right*-1, out hit, triggerRange) && Time.time > nextFire)
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
