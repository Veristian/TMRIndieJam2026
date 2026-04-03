using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class Predator : Agent
{
    public enum State
    {
        Patrol,
        Chase,
        Attack,
        GiveUp,
        Return
    }

    public State currentState;

    [Header("References")]
    // public Transform player;
    public List<Transform> patrolPoints = new List<Transform>();
    // public BoxCollider attackCollider;
    private Animator animator;

    private int patrolIndex;
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;

    [Header("Ranges")]
    public float detectRange = 10f;
    public float attackRange = 2f;

    [Header("Timers")]
    public float patrolTimer;
    public float attackTimer;

    [Header("Line of Sight")]
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public Transform eyePoint; // where the ray starts (head)

    private float patrolLimit = 10f;

    private Vector3 startPosition;


    // private bool attacking = false;
    // private CinemachineImpulseSource cinemachineCollisionImpulseSource;
    // private bool hasDamagedPlayer = false;

    private void Start()
    {
        // cinemachineCollisionImpulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        currentState = State.Patrol;
        if (patrolPoints.Count == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject pointObj = new GameObject($"PatrolPoint_{i + 1}");
                pointObj.transform.SetParent(transform);
                pointObj.transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                patrolPoints.Add(pointObj.transform);
            }
        }
        

        GoToNextPatrol();
    }
    

    void FixedUpdate()
    {
        //add resistance 
        if (!(currentState == State.Attack))
        {
            rb.isKinematic = true;
        }

    }

    protected override void Update()
    {
        base.Update();
        float distToPlayer = Vector3.Distance(transform.position, PlayerAttribute.PlayerTransform.position);
        
        switch (currentState)
        {
            case State.Patrol:
                Patrol(distToPlayer);
                PlayerAttribute.IsChasedByPredators = false;
                break;

            case State.Chase:
                Chase(distToPlayer);
                PlayerAttribute.IsChasedByPredators = true;
                break;

            case State.Attack:
                Attack(distToPlayer);
                PlayerAttribute.IsChasedByPredators = true;
                break;

            case State.GiveUp:
                GiveUp();
                PlayerAttribute.IsChasedByPredators = false;
                break;

            case State.Return:
                Return();
                PlayerAttribute.IsChasedByPredators = false;
                break;
        }

        if (sprite != null && agent.velocity.magnitude > 0.1f)
        {
            sprite.localScale = new Vector3(sprite.localScale.x, Mathf.Sign(agent.velocity.x) * Mathf.Abs(sprite.localScale.y), sprite.localScale.z);
        }
        // else if (sprite != null)
        // {
        //     sprite.localScale = new Vector3(sprite.localScale.x, Mathf.Sign(agent.velocity.x) * Mathf.Abs(sprite.localScale.y), sprite.localScale.z);
        // }
    }

    void Patrol(float dist)
    {
        if (isDisabled) return;
        patrolTimer += Time.deltaTime;

        if (dist <= detectRange + transform.localScale.x * col.radius)
        {
            currentState = State.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrol();
        }

        if (patrolTimer > patrolLimit)
        {
            currentState = State.GiveUp;
        }
    }

    void Chase(float dist)
    {
        if (isDisabled) return;
        MoveTo(PlayerAttribute.PlayerTransform.position);
        patrolTimer = 0f;

        if (dist > (detectRange + transform.localScale.x * col.radius) * 1.5f || PlayerAttribute.IsHiddenFromPredators)
        {
            currentState = State.Patrol;
            return;
        }

        if (dist <= attackRange + transform.localScale.x * col.radius)
        {
            currentState = State.Attack;
            attackTimer = attackTimer + attackCooldown - attackCooldown/4f; 
        }
    }

    void Attack(float dist)
    {
        if (isDisabled) return;
        MoveTo(transform.position); // stop moving
        attackTimer += Time.deltaTime;
        if (CanSeePlayer())
        {
            transform.LookAt(new Vector3(PlayerAttribute.PlayerTransform.position.x, transform.position.y, PlayerAttribute.PlayerTransform.position.z));
        }
        if (dist > attackRange + transform.localScale.x * col.radius || !CanSeePlayer())
        {
            currentState = State.Chase;
            return;
        }

        if (attackTimer >= attackCooldown && CanSeePlayer())
        {
            
            StartChomp();
            attackTimer = 0f;
        }
    }

    void StartChomp()
    {
        if (isDisabled) return;
        animator.SetTrigger("EatTrigger");
    }

    public override void DisableAgent()
    {
        currentState = State.GiveUp;
        TeleportToSpawn();
        base.DisableAgent();
    }

    // public void Chomp()
    // {
    //     Debug.Log("Chomp Attack!");
    //     //BoxCast to detect player
    //     Physics.BoxCast(attackCollider.bounds.center, attackCollider.bounds.extents, transform.forward, out RaycastHit hit, transform.rotation, attackRange, playerMask);
    //     if (hit.collider == null) {
    //         Debug.Log("Attack missed!");
    //         return;
    //     }
    //     PlayerAttribute.TakeDamage(damageAmount); // Example damage value
    //     cinemachineCollisionImpulseSource.GenerateImpulseWithVelocity((PlayerAttribute.PlayerTransform.position - transform.position).normalized * 5f);
    // }

    // void Lunge()
    // {
    //     Debug.Log("Lunge Attack!");
    //     rb.isKinematic = false;
    //     // Optional: quick forward burst
    //     Vector3 dir = (PlayerAttribute.PlayerTransform.position - transform.position).normalized;
    //     rb.AddForce(dir * 20f, ForceMode.VelocityChange);

    //     attacking = true;
    //     Invoke(nameof(EndAttack), 0.5f); // Attack lasts 0.5 seconds
    // }

    // void EndAttack()
    // {
    //     attacking = false;
    //     rb.isKinematic = true;
    //     hasDamagedPlayer = false;
    // }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (attacking && !hasDamagedPlayer)
    //     {
    //         if (collision.gameObject.CompareTag("Player"))
    //         {
    //             Debug.Log("Collided with Player during attack!");
    //             PlayerAttribute.TakeDamage(damageAmount); // Example damage value
    //             cinemachineCollisionImpulseSource.GenerateImpulseWithVelocity((PlayerAttribute.PlayerTransform.position - transform.position).normalized * 5f);
    //             hasDamagedPlayer = true;
            
    //         }
    //     }
    // }

    void GiveUp()
    {
        if (isDisabled) return;
        MoveTo(startPosition);
        currentState = State.Return;
    }

    void Return()
    {
        if (isDisabled) return;
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolTimer = 0f;
            currentState = State.Patrol;
            GoToNextPatrol();
        }
    }

    void GoToNextPatrol()
    {
        if (isDisabled) return;
        if (patrolPoints.Count == 0) return;

        MoveTo(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Count;
    }

    bool CanSeePlayer()
    {
        if (isDisabled) return false;
        Vector3 direction = (PlayerAttribute.PlayerTransform.position - eyePoint.position).normalized;
        float distance = Vector3.Distance(eyePoint.position, PlayerAttribute.PlayerTransform.position);

        Ray ray = new Ray(eyePoint.position, direction);
        RaycastHit hit;
        if (PlayerAttribute.IsHiddenFromPredators)
            return false;
        bool isPlayerInFront = Vector3.Dot(transform.forward, direction) > 0;
        if (!isPlayerInFront)
            return false;
        
        if (Physics.Raycast(ray, out hit, distance, obstacleMask | playerMask))
        {
            // Check if first thing hit is player
            if (((1 << hit.collider.gameObject.layer) & playerMask) != 0 && !PlayerAttribute.IsHiddenFromPredators)
            {
                return true;
            }
        }

        Debug.DrawRay(eyePoint.position, 
              (PlayerAttribute.PlayerTransform.position - eyePoint.position).normalized * (attackRange + transform.localScale.x * col.radius), 
              Color.red);

        return false;
    }

    void TeleportToSpawn()
    {
        transform.position = startPosition;
    }
}