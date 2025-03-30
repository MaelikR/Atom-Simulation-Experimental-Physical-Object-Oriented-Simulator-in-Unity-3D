using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class FishBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float swimSpeed = 2f;
    public float turnSpeed = 2f;
    public float directionChangeInterval = 3f;

    [Header("Water Limits")]
    public float waterSurfaceY = 0f;
    public float maxDepth = 5f;

    [Header("Group Behavior")]
    public float neighborRadius = 2f;
    public float separationRadius = 1f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float separationWeight = 1.5f;

    [Header("Life Settings")]
    public float maxLifeTime = 30f;
    public float floatUpSpeed = 0.5f;
    public float deadRotationSpeed = 0.2f;
    public bool IsDead() => isDead;

    [Header("Environmental Stimuli")]
    public Transform player;
    public Transform lightSource;
    public List<string> interestingAtomsTags;
    public float fleeRadius = 5f;
    public float curiosityRadius = 3f;
    public float lightAttractionRadius = 4f;

    private float age = 0f;
    private bool isDead = false;

    private Rigidbody rb;
    private Vector3 swimDirection;
    private float timeSinceLastTurn;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 1f;
        animator = GetComponent<Animator>();

        ChooseNewDirection();
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            Vector3 floatUp = new Vector3(0f, floatUpSpeed, 0f);
            Vector3 nextPos = rb.position + floatUp * Time.fixedDeltaTime;
            nextPos.y = Mathf.Min(nextPos.y, waterSurfaceY);
            rb.MovePosition(nextPos);

            Quaternion tilt = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 90f);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, tilt, deadRotationSpeed * Time.fixedDeltaTime));
            return;
        }

        Vector3 stimuliDir = ComputeStimuliResponse();
        Vector3 groupDir = ComputeGroupBehavior();
        swimDirection = (swimDirection + stimuliDir + groupDir).normalized;

        Vector3 newPosition = rb.position + swimDirection * swimSpeed * Time.fixedDeltaTime;
        float minY = waterSurfaceY - maxDepth;
        float maxY = waterSurfaceY;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        rb.MovePosition(newPosition);

        Quaternion targetRotation = Quaternion.LookRotation(-swimDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
    }

    void Update()
    {
        timeSinceLastTurn += Time.deltaTime;
        if (timeSinceLastTurn >= directionChangeInterval)
        {
            ChooseNewDirection();
            timeSinceLastTurn = 0f;
        }
        if (isDead) return;

        age += Time.deltaTime;
        if (age >= maxLifeTime)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.drag = 0.5f;
        swimDirection = Vector3.zero;
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    void ChooseNewDirection()
    {
        swimDirection = Random.onUnitSphere;
        swimDirection.y = Mathf.Clamp(swimDirection.y, -0.3f, 0.3f);
    }

    Vector3 ComputeGroupBehavior()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);

        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == this.gameObject || neighbor.GetComponent<FishBehavior>() == null)
                continue;

            Vector3 toNeighbor = neighbor.transform.position - transform.position;
            float distance = toNeighbor.magnitude;

            cohesion += neighbor.transform.position;
            alignment += neighbor.GetComponent<Rigidbody>().velocity;

            if (distance < separationRadius)
            {
                separation -= toNeighbor / distance;
            }

            count++;
        }

        if (count == 0) return Vector3.zero;

        cohesion = ((cohesion / count) - transform.position).normalized * cohesionWeight;
        alignment = (alignment / count).normalized * alignmentWeight;
        separation = separation.normalized * separationWeight;

        return cohesion + alignment + separation;
    }

    Vector3 ComputeStimuliResponse()
    {
        Vector3 response = Vector3.zero;

        // Flee from player
        if (player != null && Vector3.Distance(transform.position, player.position) < fleeRadius)
        {
            response += (transform.position - player.position).normalized * 2f;
        }

        // Follow light
        if (lightSource != null && Vector3.Distance(transform.position, lightSource.position) < lightAttractionRadius)
        {
            response += (lightSource.position - transform.position).normalized * 1.5f;
        }

        // Curious toward atoms
        foreach (string tag in interestingAtomsTags)
        {
            GameObject[] atoms = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject atom in atoms)
            {
                if (Vector3.Distance(transform.position, atom.transform.position) < curiosityRadius)
                {
                    response += (atom.transform.position - transform.position).normalized;
                }
            }
        }

        return response;
    }
}
