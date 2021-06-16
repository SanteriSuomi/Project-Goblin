using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    public float speed = 9f;
    public Rigidbody rb;

    void Awake()
    {
        //if(transform.parent.rotation.y > 0) {
        //transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit " + other.gameObject.name);
        if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
        {
            comp.ModifyHealth(-damage);
        }
        Destroy(this.gameObject);
    }
}
