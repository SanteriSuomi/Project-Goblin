using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float startingHealth = 100;

    private float health;
    public HealthBar healthBar;

    public float Health => health;

    private void Awake()
    {
        health = startingHealth;
        healthBar.SetHealth(health);
    }

    public void ModifyHealth(float by) // Modify health by negative or positive value
    {
        health += by;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}
