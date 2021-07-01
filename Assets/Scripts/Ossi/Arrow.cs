using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    public Rigidbody rb;
    public GameObject player;

    public  float speed;
    public  float chargeTime;

    //private PlayerBow playerBow;


    void Awake()
    {
    	//playerBow = player.GetComponent<PlayerBow>();
    	//speed = playerBow.speed;
    	//chargeTime = playerBow.chargeTime;

        //if(transform.parent.rotation.y > 0) {
        //transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
    	rb.velocity = (transform.forward * speed) + transform.up * 2f;
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.gameObject.name);
        //if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
        //{
        //    comp.ModifyHealth(-damage);
        //}
        Destroy(this.gameObject);
    }
}
