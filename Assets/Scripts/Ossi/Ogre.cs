using UnityEngine.AI;
using UnityEngine;

public class Ogre : MonoBehaviour
{
	public Transform player;
	public HealthBar healthBar;
	private float health = 1000;
	public GameObject canvas;

	public bool facingLeft = true;

	private void Awake() {
        healthBar.SetHealth(health);
        canvas.SetActive(false);
    }

	public void LookAtPlayer() {
       if(player.position.x > transform.position.x && facingLeft) {
            Flip();
            facingLeft = false;
       }
       else if (player.position.x < transform.position.x && !facingLeft) {
        Flip();
        facingLeft = true;
       }
	}

	public void ModifyHealth(float by)
    {
        health += by;
        healthBar.SetHealth(health);
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth() {
    	return health;
    }

	private void Flip()
    {
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.z *= -1;
        transform.localScale = theScale;
    }

    public void ShowHP() {
    	canvas.SetActive(true);
    }

}
