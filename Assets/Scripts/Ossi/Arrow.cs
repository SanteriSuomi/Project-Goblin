using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    public Rigidbody rb;
    //public GameObject player;

    [SerializeField]
    float cooldownUntilDestroy = 10;
    WaitForSeconds cooldownUntilDestroyWFS;

    public float speed;
    public float chargeTime;

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
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            rb.isKinematic = true;
            rb.freezeRotation = true;
            transform.SetParent(other.transform, true);
            enemy.ModifyHeath(-damage);
            Debug.Log("Arrow hit player");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Hit " + other.gameObject.name);
    //     //if (other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth comp))
    //     //{
    //     //    comp.ModifyHealth(-damage);
    //     //}
    //     Destroy(this.gameObject);
    // }
}
