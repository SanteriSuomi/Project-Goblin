using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float startingHealth = 100;

    private float health;
    HealthBar healthBar;

    public float Health => health;

    private void Awake()
    {
        health = startingHealth;
        healthBar = FindObjectOfType<HealthBar>();
        healthBar.SetHealth(health);
    }

    public void ModifyHealth(float by) // Modify health by negative or positive value
    {
        health += by;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Debug.Log("Player death event (health below 0)");
        }
    }
}
