using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float speed = 9f;
	public Rigidbody2D rb;

    void Start() {
    	//if(transform.parent.rotation.y > 0) {
    		//transform.localScale = new Vector3(-1f, 1f, 1f);
    	//}
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo) {
    	Debug.Log(hitInfo.name);
    	Destroy(gameObject);
    }
}
