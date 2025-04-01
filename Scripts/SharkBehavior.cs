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
    public float detectionRadius = 12f;
    public float attackDistance = 1.5f;
    public float directionChangeInterval = 4f;
    public float visionAngle = 120f;

    [Header("Water Limits")]
    public float waterSurfaceY = 0f;
    public float maxDepth = 5f;

    [Header("Memory")]
    public float memoryDuration = 6f;

    [Header("References")]
    public Animator animator;

    private Rigidbody rb;
    private Vector3 currentDirection;
    private float timeSinceLastTurn = 0f;
    private GameObject currentTarget;
    private float memoryTimer = 0f;

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
            memoryTimer -= Time.deltaTime;
            if (memoryTimer <= 0f)
                currentTarget = null;
            else
                ChaseAndAttack();
        }

        if (animator != null)
            animator.SetFloat("Speed", currentTarget != null ? chaseSpeed : patrolSpeed);
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
                if (IsInVision(hit.transform))
                    TrySetTarget(fish.gameObject, ref closestDist);
            }
            else if (hit.CompareTag("Player"))
            {
                if (hit.TryGetComponent(out IDamageable damageable) && IsInVision(hit.transform))
                {
                    TrySetTarget(hit.gameObject, ref closestDist);
                }
            }
        }
    }

    void TrySetTarget(GameObject target, ref float closestDist)
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist < closestDist)
        {
            currentTarget = target;
            memoryTimer = memoryDuration;
            closestDist = dist;
        }
    }

    bool IsInVision(Transform target)
    {
        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle <= visionAngle / 2f;
    }

    void ChaseAndAttack()
    {
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (dist <= attackDistance)
        {
            if (currentTarget.TryGetComponent(out FishBehavior fish))
            {
                fish.Die();
            }
            else if (currentTarget.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(45f, gameObject);
            }

            currentTarget = null;
        }
    }

    public void OnHearSound(Vector3 origin, GameObject source)
    {
        float dist = Vector3.Distance(transform.position, origin);
        if (dist < detectionRadius)
        {
            currentTarget = source;
            memoryTimer = memoryDuration;
        }
    }
}
