using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float startingHealth = 100;

    private float health;
    public HealthBar healthBar;

    public void ModifyHealth(float by) // Modify health by negative or positive value
    {
        health += by;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Debug.Log("Player death event (health below 0)");
        }
    }

    void Awake()
    {
        health = startingHealth;
        healthBar.SetHealth(health);
    }
}
