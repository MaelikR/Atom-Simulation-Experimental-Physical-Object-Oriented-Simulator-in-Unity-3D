using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LivingOrganism : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1f;
    public float turnChance = 0.01f;
    public float waterDrag = 1f;

    [Header("Pulse")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.2f;

    [Header("Life")]
    public float maxEnergy = 100f;
    public float energyLossPerSecond = 1f;
    public GameObject humanoidPrefab;

    private Rigidbody rb;
    private Vector3 direction;
    private Vector3 baseScale;
    private float currentEnergy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Random.onUnitSphere;
        baseScale = transform.localScale;
        currentEnergy = maxEnergy;

        rb.useGravity = false;
        rb.drag = waterDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        Wander();
        Pulse();
        DrainEnergy();

        if (currentEnergy <= 0f)
        {
            TryMutate();
        }
    }

    void Wander()
    {
        if (Random.value < turnChance)
        {
            direction = Random.insideUnitSphere.normalized;
        }
    }

    void Pulse()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = baseScale * pulse;
    }

    void DrainEnergy()
    {
        currentEnergy -= energyLossPerSecond * Time.deltaTime;
    }

    void TryMutate()
    {
        if (humanoidPrefab != null)
        {
            Instantiate(humanoidPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
