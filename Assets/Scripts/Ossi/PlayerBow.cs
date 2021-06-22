using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Rigidbody bulletRb;

    public float chargeTime;
    public float speed;

    private float nextFire = 0f;

    bool shot = false;

    private Arrow arrow;

    //void FixedUpdate()
    //{
    //	if(Input.GetButton("Fire1") && Time.time > nextFire) {
    //	    shoot();
    //	    nextFire = Time.time + 0.2f;
    //	}
    //}
    void Awake() {
    	speed = 6f;
    	chargeTime = 0f;
    	arrow = bulletPrefab.GetComponent<Arrow>();
    }

    void Update() {
    	if(Input.GetButton("Fire1")) {
    		chargeTime += Time.deltaTime;
    		shot = false;
    	}

    	if(Input.GetButtonUp("Fire1") && (chargeTime > 1f)) {
    		if(chargeTime > 2.5f) {
    			speed = speed*5f;
    			arrow.speed = speed;
    			shoot();
    		}
    		else{
    			speed = speed * (chargeTime * 2);
    			arrow.speed = speed;
    			shoot();
    		}
	 	}

	 	if(shot) {
	 		chargeTime = 0f;
    		speed = 5f;
	 	}
    }

    void shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        Debug.Log(speed);
        shot = true;
    }
}
