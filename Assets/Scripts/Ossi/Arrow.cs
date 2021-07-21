using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    float damage = 10f;
    public Rigidbody rb;
    Collider col;

    [SerializeField]
    float cooldownUntilDestroy = 10;
    WaitForSeconds cooldownUntilDestroyWFS;

    public float speed;
    public float chargeTime;

    void Awake()
    {
        col = GetComponent<Collider>();
        rb.velocity = (transform.forward * speed) + transform.up * 2f;
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MageProjectile"))
        {
            Physics.IgnoreCollision(this.col, other.collider, true);
            return;
        }

        if (other.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            rb.isKinematic = true;
            rb.freezeRotation = true;
            transform.SetParent(other.transform, true);
            enemy.ModifyHealth(-damage);
            Debug.Log("Arrow hit player");
        }
        else
        {
            rb.isKinematic = true;
            rb.freezeRotation = true;
        }
    }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }
}
