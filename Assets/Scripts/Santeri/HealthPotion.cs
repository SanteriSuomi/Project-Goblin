using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField]
    float healAmount = 50;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform != null && other.transform.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            health.ModifyHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
