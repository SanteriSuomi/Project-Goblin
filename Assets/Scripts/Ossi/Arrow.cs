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

    TrailRenderer trail;

    public float speed;
    public float chargeTime;

    void Awake()
    {
        col = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();
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
            Freeze();
            transform.SetParent(other.transform, true);
            enemy.ModifyHealth(-damage);
        }
        if (other.gameObject.TryGetComponent<Ogre>(out Ogre ogre))
        {
            Freeze();
            transform.SetParent(other.transform, true);
            ogre.ModifyHealth(-damage);
        }
        else
        {
            Freeze();
        }
    }

    void Freeze()
    {
        col.isTrigger = true;
        rb.isKinematic = true;
        rb.freezeRotation = true;
        trail.emitting = false;
    }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }
}