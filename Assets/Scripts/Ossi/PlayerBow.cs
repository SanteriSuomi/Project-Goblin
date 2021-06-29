using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public Transform firePoint;
    public Transform aim;
    public GameObject bulletPrefab;
    private Rigidbody bulletRb;
    public LayerMask mouseAimMask;

    Vector3 mousePoint;

    public float chargeTime;
    public float speed;

    //private float nextFire = 0f;

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

    	Aim();

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
        Instantiate(bulletPrefab, firePoint.position, aim.rotation);
        Debug.Log(speed);
        shot = true;
    }

    void Aim() {
    	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    	RaycastHit hit;

    	if(Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask)) {
    		mousePoint = hit.point;
    		mousePoint.z = 0;
    		aim.LookAt(mousePoint);
    	}
    }

    public Vector3 GetMouseWorldPosition() {
    	Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    	vec.z = 0f;
    	return vec;
    }
    public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
    	Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    	worldPosition.z = transform.position.z;
    	return worldPosition; 
    }
}
