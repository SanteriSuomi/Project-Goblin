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

    [SerializeField]
    float damagePerHit = 15;
    [SerializeField]
    float attackSpeed = 1;

    float attackTimer;
    float chaseTimer;

    [SerializeField]
    float wanderSpeed = 2.25f;
    [SerializeField]
    float chaseSpeed = 3f;

    [SerializeField]
    float chasePathUpdateSpeed = 1f;

    [SerializeField]
    float offsetLength = 1.4f;

    PlayerHealth player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        chaseTimer = chasePathUpdateSpeed + 0.01f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        ForceZ();
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

    void ForceZ()
    {
        if (transform.position.z < 0 || transform.position.z > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
        chaseTimer += Time.deltaTime;
        if (chaseTimer >= chasePathUpdateSpeed)
        {
            agent.SetDestination(GetPlayerPosAndOffset());
            chaseTimer = 0;
            Debug.Log("Update path");
        }
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            Debug.Log("Dealt " + damagePerHit + " damage to player.");
            player.ModifyHealth(-damagePerHit);
            attackTimer = 0;
        }
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
            if (state == State.Attack)
            {
                return;
            }
            state = State.Attack;
            chaseTimer = chasePathUpdateSpeed + 0.01f;
            Debug.Log("Attack");
        }
        else if (distance < distanceToPlayerForChase || angle < angleToPlayerForChase) // Chase
        {
            if (state == State.Chase)
            {
                return;
            }
            agent.ResetPath();
            state = State.Chase;
            attackTimer = attackSpeed + 0.01f;
            agent.speed = chaseSpeed;
            Debug.Log("Chase");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (state == State.Wander)
            {
                return;
            }
            agent.ResetPath();
            state = State.Wander;
            attackTimer = attackSpeed + 0.01f;
            chaseTimer = chasePathUpdateSpeed + 0.01f;
            agent.speed = wanderSpeed;
            Debug.Log("Wander");
        }
    }

    Vector3 GetPlayerPosAndOffset()
    {
        return player.transform.position + ((transform.position - player.transform.position).normalized * offsetLength);
    }
}
