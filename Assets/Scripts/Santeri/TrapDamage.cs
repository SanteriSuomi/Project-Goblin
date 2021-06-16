using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField]
    float damage = 25;

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision event with " + gameObject.name + " and " + other.gameObject.name);
        PlayerHealth player = other.transform.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.ModifyHealth(-damage);
            Debug.Log("Damaged player by " + -damage);
        }
    }
}
