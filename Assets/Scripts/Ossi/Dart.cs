using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    public float speed = 9f;
    public Rigidbody rb;

    [SerializeField]
    float cooldownUntilDestroy = 10;
    WaitForSeconds cooldownUntilDestroyWFS;

    void Awake()
    {
        //if(transform.parent.rotation.y > 0) {
        //transform.localScale = new Vector3(-1f, 1f, 1f);
        //}
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        rb.velocity = transform.forward * speed;
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Hit " + other.gameObject.name);
        if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
        {
            comp.ModifyHealth(-damage);
        }
        Destroy(this.gameObject);
    }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }
}
