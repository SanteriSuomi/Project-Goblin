using UnityEngine.AI;
using UnityEngine;

public class Goblin : Enemy
{
    NavMeshAgent agent;

    [SerializeField]
    float wanderRange = 7.5f;

    [SerializeField]
    float angleToPlayerForChase = 15;
    [SerializeField]
    float distanceToPlayerForChase = 7.5f;

    [SerializeField]
    float distanceToPlayerForAttack = 1.5f;

    Vector3 playerPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Wander:
                Wander();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    protected override void Wander()
    {
        if (!agent.hasPath)
        {
            float randX = Random.Range(-wanderRange, wanderRange);
            agent.SetDestination(transform.position + (Vector3.right * randX));
        }
    }

    protected override void Chase()
    {
        if (!agent.hasPath)
        {
            agent.SetDestination(playerPosition);
        }
    }

    protected override void Attack()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerContact(other);
        }
    }

    void PlayerContact(Collider player)
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        float angle = Vector3.Angle((player.transform.position - transform.position), transform.forward);
        if (distance < distanceToPlayerForAttack) // Attack
        {
            // state = State.Attack;
            Debug.Log("Attack");
        }
        else if (distance < distanceToPlayerForChase || angle < angleToPlayerForChase) // Chase
        {
            Vector3 offset = ((transform.position - player.transform.position).normalized * 1.5f);
            playerPosition = player.transform.position + offset;
            Debug.DrawLine(transform.position, playerPosition);
            agent.ResetPath();
            state = State.Chase;
            Debug.Log("Chase");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agent.ResetPath();
            state = State.Wander;
            Debug.Log("Wander");
        }
    }
}
