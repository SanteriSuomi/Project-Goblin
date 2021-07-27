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
    Rigidbody rb;
    Collider col;
    SpriteRenderer spr;
    public Transform PlayerTransform { get; set; }
    private Vector3 velocity;
    public Collider GoblinCollider { get; set; }

    private void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        spr = GetComponent<SpriteRenderer>();
        cooldownUntilDestroyWFS = new WaitForSeconds(cooldownUntilDestroy);
        StartCoroutine(nameof(DestroyCooldown));
    }

    private void Start()
    {
        velocity = ((PlayerTransform.position + Vector3.up) - transform.position).normalized;
        if (velocity.x > 0)
        {
            spr.flipX = false;
        }
        Physics.IgnoreCollision(GoblinCollider, col, true);
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

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (velocity * speed * Time.deltaTime));
    }

    // private void Update()
    // {
    //     transform.position = Vector3.MoveTowards(transform.position, MovePosition, Time.deltaTime * speed);
    // }

    IEnumerator DestroyCooldown()
    {
        yield return cooldownUntilDestroyWFS;
        Destroy(gameObject);
    }
}
