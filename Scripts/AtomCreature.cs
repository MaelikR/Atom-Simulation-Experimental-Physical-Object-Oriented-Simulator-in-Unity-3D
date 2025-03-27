// AtomCreature.cs — transforme un atome en créature vivante
using UnityEngine;
using Fusion;

public class  : NetworkBehaviour
{
    public float wanderRadius = 5f;
    public float wanderSpeed = 2f;
    public float turnSpeed = 2f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 targetPosition;
    private Rigidbody rb;
    private float baseY;

    public override void Spawned()
    {
        rb = GetComponent<Rigidbody>();
        baseY = transform.position.y;
        PickNewTarget();
    }

    void FixedUpdate()
    {
        if (!HasStateAuthority) return;

        // Léger flottement
        Vector3 floatOffset = Vector3.up * Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // Déplacement vers la cible
        Vector3 dir = (targetPosition - transform.position);
        if (dir.magnitude < 0.5f)
            PickNewTarget();

        Vector3 move = dir.normalized * wanderSpeed;
        rb.velocity = new Vector3(move.x, 0, move.z) + floatOffset;
    }

    void PickNewTarget()
    {
        Vector2 randCircle = Random.insideUnitCircle * wanderRadius;
        targetPosition = new Vector3(transform.position.x + randCircle.x, baseY, transform.position.z + randCircle.y);
    }
}
