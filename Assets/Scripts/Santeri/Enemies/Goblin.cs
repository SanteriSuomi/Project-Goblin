using UnityEngine.AI;
using UnityEngine;

enum Type
{
    Melee,
    Mage
}

public class Goblin : Enemy
{
    NavMeshAgent agent;
    Animator anim;

    [SerializeField]
    Type type;

    [SerializeField]
    GameObject mageFireBallPrefab;
    [SerializeField]
    Transform fireBallSpawnPoint;

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


    [SerializeField]
    LayerMask enviroMask;
    [SerializeField]
    float enviroCollisionCooldown = 1.5f;
    float enviroCollisionTimer = 0;

    PlayerHealth player;
    Collider col;


    private void Awake()
    {
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        chaseTimer = chasePathUpdateSpeed + 0.01f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        anim = GetComponent<Animator>();
        if (type == Type.Mage)
        {
            attackTimer = attackSpeed + 0.01f;
        }
    }

    private void Update()
    {
        CheckEnvironmentCollision();
        ForceZ();
        if (type == Type.Mage) return;
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

    void CheckEnvironmentCollision()
    {
        enviroCollisionTimer += Time.deltaTime;
        bool rayLeft = Physics.Raycast(transform.position, Vector3.left, out RaycastHit hitLeft, 1, enviroMask);
        bool rayRight = Physics.Raycast(transform.position, Vector3.right, out RaycastHit hitRight, 1, enviroMask);
        if (state == State.Wander && enviroCollisionTimer >= enviroCollisionCooldown && (rayLeft || rayRight)
            && hitLeft.transform != null && hitRight.transform != null && !hitLeft.transform.CompareTag("Player") && !hitRight.transform.CompareTag("Player"))
        {
            Vector3 hitPos = rayLeft ? hitLeft.transform.position : hitRight.transform.position;
            Vector3 dist = (hitPos - transform.position).normalized;
            if (dist.x > 0)
            {
                SetRandomPath(0, wanderRange);
            }
            else
            {
                SetRandomPath(-wanderRange, 0);
            }
            enviroCollisionTimer = 0;
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
            SetRandomPath(-wanderRange, wanderRange);
        }
    }

    void SetRandomPath(float negWanderRange, float posWanderRange)
    {
        float randX = Random.Range(negWanderRange, posWanderRange);
        agent.SetDestination(transform.position + (Vector3.right * randX));
    }

    protected override void Chase()
    {
        chaseTimer += Time.deltaTime;
        if (chaseTimer >= chasePathUpdateSpeed)
        {
            agent.SetDestination(GetPlayerPosAndOffset());
            chaseTimer = 0;
            // Debug.Log("Update path");
        }
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
            // Debug.Log("Dealt " + damagePerHit + " damage to player.");
            player.ModifyHealth(-damagePerHit);
            attackTimer = 0;
        }
    }

    private void OnTriggerStay(Collider contact)
    {
        if (!contact.CompareTag("Player"))
        {
            return;
        }
        Vector3 offset = Vector3.up * 1.3f;
        Physics.Linecast(transform.position + offset, player.transform.position + offset, out RaycastHit hit);
        if (hit.transform == null || (hit.transform != null && !hit.transform.CompareTag("Player")))
        {
            if (type == Type.Melee)
            {
                state = State.Wander;
            }
            return;
        }
        float distance = Vector3.Distance(contact.transform.position, transform.position);
        switch (type)
        {
            case Type.Melee:
                if (distance > distanceToPlayerForChase) return;
                PlayerContactMelee(contact, distance);
                break;
            case Type.Mage:
                PlayerContactMage(contact);
                break;
        }
    }

    void PlayerContactMelee(Collider player, float distance)
    {
        float angle = Vector3.Angle((player.transform.position - transform.position), transform.forward);
        if (distance < distanceToPlayerForAttack) // Attack
        {
            if (state == State.Attack)
            {
                return;
            }
            state = State.Attack;
            chaseTimer = chasePathUpdateSpeed + 0.01f;
            anim.SetTrigger("StartMelee");
            // Debug.Log("Attack");
        }
        else if (distance < distanceToPlayerForChase || angle < angleToPlayerForChase) // Chase
        {
            if (state == State.Chase)
            {
                return;
            }
            if (state == State.Attack)
            {
                anim.SetTrigger("EndMelee");
            }
            agent.ResetPath();
            state = State.Chase;
            attackTimer = attackSpeed + 0.01f;
            agent.speed = chaseSpeed;
            // Debug.Log("Chase");
        }
    }

    void PlayerContactMage(Collider player)
    {
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer >= attackSpeed)
        {
            anim.SetTrigger("StartCast");
            GameObject fireBall = Instantiate(mageFireBallPrefab, fireBallSpawnPoint.position, Quaternion.identity);
            GoblinFireBall fireBallComp = fireBall.GetComponent<GoblinFireBall>();
            fireBallComp.PlayerTransform = player.transform;
            fireBallComp.GoblinCollider = col;
            attackTimer = 0;
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
            if (state == State.Attack)
            {
                anim.SetTrigger("EndMelee");
            }
            agent.ResetPath();
            state = State.Wander;
            attackTimer = attackSpeed + 0.01f;
            chaseTimer = chasePathUpdateSpeed + 0.01f;
            agent.speed = wanderSpeed;
        }
    }

    Vector3 GetPlayerPosAndOffset()
    {
        return player.transform.position + ((transform.position - player.transform.position).normalized * offsetLength);
    }
}