using UnityEngine.AI;
using UnityEngine;

public class Ogre : MonoBehaviour
{
    Transform player;
    public HealthBar healthBar;
    private float health = 1000;
    public GameObject canvas;
    public bool intro;
    AudioManager audioManager;

    public bool facingLeft = true;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar.SetHealth(health);
        canvas.SetActive(false);
        intro = false;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void LookAtPlayer()
    {
        if (player.position.x > transform.position.x && facingLeft)
        {
            Flip();
            facingLeft = false;
        }
        else if (player.position.x < transform.position.x && !facingLeft)
        {
            Flip();
            facingLeft = true;
        }
    }

    public void ModifyHealth(float by)
    {
        health += by;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return health;
    }

    private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    public void ShowHP()
    {
        canvas.SetActive(true);
        Debug.Log("hp should show");
    }

    public void StepSound() {
    	if(intro) {
    		audioManager.Play("Drop");
    	}
    }

}
