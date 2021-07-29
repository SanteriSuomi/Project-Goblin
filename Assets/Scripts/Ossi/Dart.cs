using System.Collections;
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

    [SerializeField]
    LayerMask shootLayerMask;

    public void SetVelocity(Vector3 vel)
    {
        rb.velocity = vel * speed;
    }

    void Awake()
    {
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (shootLayerMask != (shootLayerMask | (1 << other.gameObject.layer)))
        {
            return;
        }
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
