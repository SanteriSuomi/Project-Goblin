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
    AudioManager audioManager;

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

    [SerializeField]
    LayerMask linecastMask;

    private void Awake()
    {
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        chaseTimer = chasePathUpdateSpeed + 0.01f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        audioManager = FindObjectOfType<AudioManager>();
        anim = GetComponent<Animator>();
        if (type == Type.Mage)
        {
            attackTimer = attackSpeed + 0.01f;
        }
    }

    private void Update()
    {
        ForceZ();
        if (type == Type.Mage) return;
        CheckEnvironmentCollision();
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
        bool rayLeft = Physics.Raycast(transform.position + (Vector3.down * 0.5f), Vector3.left, out RaycastHit hitLeft, 1, enviroMask);
        bool rayRight = Physics.Raycast(transform.position + (Vector3.down * 0.5f), Vector3.right, out RaycastHit hitRight, 1, enviroMask);
        bool rayBottomLeft = Physics.Raycast(transform.position + (Vector3.down * 0.5f), new Vector3(-0.5f, -0.5f, 0), out RaycastHit hitBottomLeft, 1, enviroMask);
        bool rayBottomRight = Physics.Raycast(transform.position + (Vector3.down * 0.5f), new Vector3(0.5f, 0.5f, 0), out RaycastHit hitBottomRight, 1, enviroMask);
        if (state == State.Wander && enviroCollisionTimer >= enviroCollisionCooldown)
        {
            if ((rayLeft || rayRight) && hitLeft.transform != null && hitRight.transform != null && !hitLeft.transform.CompareTag("Player") && !hitRight.transform.CompareTag("Player"))
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
            else if (!rayBottomLeft || !rayBottomRight)
            {
                if (rayBottomLeft)
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
        }
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackSpeed)
        {
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
        if (type == Type.Mage)
        {
            RotateTowardsPlayer();
        }
        Vector3 yOffsetSelf = Vector3.up * 1.3f;
        Vector3 yOffsetPlayer = Vector3.up * 1.15f;
        Vector3 playerOffset = ((transform.position + yOffsetSelf) - (player.transform.position + yOffsetPlayer)).normalized * 0.125f;
        Physics.Linecast(transform.position + (transform.forward * 0.4f) + yOffsetSelf, player.transform.position + yOffsetPlayer + playerOffset,
                    out RaycastHit hit, linecastMask, QueryTriggerInteraction.Ignore);
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

    void RotateTowardsPlayer()
    {
        Quaternion lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        lookRotation.x = 0;
        lookRotation.z = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 3);
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
        }
    }

    void PlayerContactMage(Collider player)
    {
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer >= attackSpeed)
        {
            anim.SetTrigger("StartCast");
            audioManager.Play("GoblinCast");
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