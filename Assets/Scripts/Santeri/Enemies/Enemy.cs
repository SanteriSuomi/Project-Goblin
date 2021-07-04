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

    public void ModifyHeath(float by)
    {
        health += by;
        if (health < 0)
        {
            Destroy(gameObject);
        }
    }

    protected State state = State.Wander;

    protected abstract void Wander();
    protected abstract void Chase();
    protected abstract void Attack();
}
