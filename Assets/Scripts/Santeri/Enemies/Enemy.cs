using UnityEngine;

public enum State
{
    Wander,
    Chase,
    Attack
}

// Base script for enemies
public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float health = 100;
    public HealthBar healthBar;
    [SerializeField]
    float healthDropChance = 0.25f;

    [SerializeField]
    GameObject healthDrop;

    private void Awake()
    {
        healthBar.SetHealth(health);
    }

    public void ModifyHealth(float by)
    {
        health += by;
        healthBar.SetHealth(health);
        if (health < 0)
        {
            if (Random.Range(0f, 1f) < healthDropChance)
            {
                Instantiate(healthDrop, transform.position + (Vector3.up * 0.4f), Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    protected State state = State.Wander;

    protected abstract void Wander();
    protected abstract void Chase();
    protected abstract void Attack();
}
