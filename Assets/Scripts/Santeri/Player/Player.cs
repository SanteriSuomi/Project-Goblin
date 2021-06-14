using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float startingHealth = 100;

    private float health;

    public void ModifyHealth(float by) // Modify health by negative or positive value
    {
        health += by;
        if (health <= 0)
        {
            Debug.Log("Player death event (health below 0)");
        }
    }

    void Awake()
    {
        health = startingHealth;
    }
}
