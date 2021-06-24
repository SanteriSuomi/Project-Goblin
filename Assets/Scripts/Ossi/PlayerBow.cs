using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    public Transform firePoint;
    public Transform aim;
    public GameObject bulletPrefab;
    private Rigidbody bulletRb;

    Vector3 lookPos;

    public float chargeTime;
    public float speed;

	float angle;

   	float aimSpeed;

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
    	if(aimSpeed <= 0.005f) {
        	aimSpeed += Time.fixedDeltaTime;
        }
        else {
        	Aim();
        	aimSpeed = 0f;
        }


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
    	Vector3 mousePosition = GetMouseWorldPosition();
    	Vector3 aimDirection = (mousePosition - transform.position).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;


	    //aim.eulerAngles = new Vector3(angle, 90f, 0f);

	    aim.LookAt(Input.mousePosition);


        Debug.Log(angle);

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
