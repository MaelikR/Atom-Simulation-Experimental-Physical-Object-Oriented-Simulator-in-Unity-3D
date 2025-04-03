using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InsectGenetics))]
public class InsectBehavior : MonoBehaviour
{
    public bool canFly = false; // Détermine si c'est un insecte volant (papillon) ou non (scarabée)
    public float moveSpeed;
    public float turnSpeed;
    public float curiosityRadius;
    public float fleeRadius;
    public float hoverAmplitude = 0.1f;
    public float hoverFrequency = 2f;

    private Rigidbody rb;
    private InsectGenetics genetics;
    private Vector3 moveDirection;
    private float timeSinceLastTurn;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        genetics = GetComponent<InsectGenetics>();

        if (genetics != null && genetics.dna != null)
        {
            moveSpeed = genetics.dna.moveSpeed;
            turnSpeed = genetics.dna.turnSpeed;
            curiosityRadius = genetics.dna.curiosity;
            fleeRadius = genetics.dna.fleeDistance;
        }

        ChooseNewDirection();
        timeSinceLastTurn = Random.Range(0f, 2f);
    }

    void FixedUpdate()
    {
        timeSinceLastTurn += Time.fixedDeltaTime;
        if (timeSinceLastTurn > 3f)
        {
            ChooseNewDirection();
            timeSinceLastTurn = 0f;
        }

        Vector3 adjustedDir = moveDirection;
        if (canFly)
        {
            adjustedDir.y += Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        }
        else
        {
            adjustedDir.y = 0; // reste au sol
        }

        rb.MovePosition(rb.position + adjustedDir.normalized * moveSpeed * Time.fixedDeltaTime);
        Quaternion rot = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, turnSpeed * Time.fixedDeltaTime));
    }

    void ChooseNewDirection()
    {
        Vector3 randomDir = Random.onUnitSphere;
        if (!canFly) randomDir.y = 0; // interdit le vol aux scarabées
        moveDirection = randomDir.normalized;
    }
}
