using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour {
	public float speed = 9f;
	public Rigidbody rb;
	PlayerHealth playercs;

    void Awake() {
    	//if(transform.parent.rotation.y > 0) {
    		//transform.localScale = new Vector3(-1f, 1f, 1f);
    	//}
        rb.velocity = transform.forward * speed;
        playercs = GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider hitInfo) {
    	Debug.Log(hitInfo.name);
    	Destroy(gameObject);
    	playercs.ModifyHealth(-10f);
    }
}
