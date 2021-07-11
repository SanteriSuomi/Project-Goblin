using System.Collections;
using UnityEngine;

public class GoblinFireBall : MonoBehaviour
{
    [SerializeField]
    float damage = 15;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float cooldownUntilDestroy = 10;
    WaitForSeconds cooldownUntilDestroyWFS;
    Collider col;
    public Vector3 MovePosition { get; set; }

    private void Awake()
    {
        col = GetComponent<Collider>();
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerArrow"))
        {
            Physics.IgnoreCollision(this.col, other.collider, true);
            return;
        }

        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            player.ModifyHealth(-damage);
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, MovePosition, Time.deltaTime * speed);
    }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }
}
