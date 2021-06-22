using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    float speed = 6f;
    public Rigidbody rb;
    public GameObject player;
    private float chargeTime = 0f;

    public PlayerBow playerBow;


    void Awake()
    {
    	playerBow = 
        //if(transform.parent.rotation.y > 0) {
        //transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
    	rb.velocity = (transform.forward * speed) + transform.up * 3f;
    }


    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit " + other.gameObject.name);
        //if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
        //{
        //    comp.ModifyHealth(-damage);
        //}
        Destroy(this.gameObject);
    }
}
