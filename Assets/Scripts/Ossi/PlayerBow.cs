using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody bulletRb;

    private float chargeTime = 0f;
    private float speed = 6f;

    private float nextFire = 0f;

    //void FixedUpdate()
    //{
    //	if(Input.GetButton("Fire1") && Time.time > nextFire) {
    //	    shoot();
    //	    nextFire = Time.time + 0.2f;
    //	}
    //}
    void Awake() {
    	bulletRb = bulletPrefab.GetComponent<Rigidbody>();
    }

    void Update() {
    	if(Input.GetButton("Fire1")) {
    		chargeTime += Time.deltaTime;
    	}

    	if(Input.GetButtonUp("Fire1") && (chargeTime > 1f)) {
    		if(chargeTime > 4f) {
    			speed = speed*4f;
    			shoot();
    		}
    		else{
    			speed = speed*chargeTime;
    			shoot();
    		}
    		chargeTime = 0f;
    		speed = 6f;
	 	}
    }

    void shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        bulletRb.velocity = (bulletPrefab.transform.forward * speed) + bulletPrefab.transform.up * 3f;
        Debug.Log("Arrow shot");
    }
}
