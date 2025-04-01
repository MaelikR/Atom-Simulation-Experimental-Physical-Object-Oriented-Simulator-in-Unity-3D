using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class SharkBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 3f;
    public float chaseSpeed = 6f;
    public float turnSpeed = 3f;

    [Header("Hunting Settings")]
    public float detectionRadius = 10f;
    public float attackDistance = 1.5f;
    public float directionChangeInterval = 4f;

    [Header("Water Limits")]
    public float waterSurfaceY = 0f;
    public float maxDepth = 5f;

    private Rigidbody rb;
    private Vector3 currentDirection;
    private float timeSinceLastTurn = 0f;
    private GameObject currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 1f;

        ChooseRandomDirection();
    }

    void Update()
    {
        if (currentTarget == null)
        {
            ScanForTargets();
            Wander();
        }
        else
        {
            ChaseAndAttack();
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = currentTarget != null
            ? (currentTarget.transform.position - transform.position).normalized
            : currentDirection;

        float speed = currentTarget != null ? chaseSpeed : patrolSpeed;

        Vector3 nextPos = rb.position + moveDir * speed * Time.fixedDeltaTime;

        float minY = waterSurfaceY - maxDepth;
        float maxY = waterSurfaceY;
        nextPos.y = Mathf.Clamp(nextPos.y, minY, maxY);

        rb.MovePosition(nextPos);

        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));
    }

    void Wander()
    {
        timeSinceLastTurn += Time.deltaTime;
        if (timeSinceLastTurn >= directionChangeInterval)
        {
            ChooseRandomDirection();
            timeSinceLastTurn = 0f;
        }
    }

    void ChooseRandomDirection()
    {
        currentDirection = Random.onUnitSphere;
        currentDirection.y = Mathf.Clamp(currentDirection.y, -0.2f, 0.2f);
    }

    void ScanForTargets()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out FishBehavior fish) && !fish.IsDead())
            {
                float dist = Vector3.Distance(transform.position, fish.transform.position);
                if (dist < closestDist)
                {
                    currentTarget = fish.gameObject;
                    closestDist = dist;
                }
            }
            else if (hit.CompareTag("Player"))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < closestDist && hit.TryGetComponent(out IDamageable damageable))
                {
                    currentTarget = hit.gameObject;
                    closestDist = dist;
                }
            }
            var player = hit.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead)
            {
                float dist = Vector3.Distance(transform.position, player.transform.position);
                if (dist < closestDist)
                {
                    currentTarget = player.gameObject;
                    closestDist = dist;
                }
            }
        }
    }

    void ChaseAndAttack()
    {
        if (currentTarget == null) return;

        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (dist <= attackDistance)
        {
            if (currentTarget.TryGetComponent(out FishBehavior fish))
            {
                fish.Die();
            }
            else if (currentTarget.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(25f, gameObject);
            }

            currentTarget = null;
        }
    }
}
