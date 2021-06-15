using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dartShoot : MonoBehaviour {
	public Transform firePoint;
	public GameObject bulletPrefab;

    void Update() {
    	if(Input.GetButtonDown("Fire1")) {
    		shoot();
    	}
    }

    void shoot() {
    	Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
