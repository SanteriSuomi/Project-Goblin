using UnityEngine;

public abstract class Chest : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerHealth>(out PlayerHealth player))
        {
            OnUse(player);
        }
    }

    public abstract void OnUse(PlayerHealth player);
}
