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

    [Header("Mutation Settings")]
    public GameObject[] possibleMutations;
    public float mutationDelay = 1.5f;
    public float mutationFloatSpeed = 0.5f;
    public float mutationFlashSpeed = 10f;

    private Rigidbody rb;
    private Vector3 direction;
    private Vector3 baseScale;
    private float currentEnergy;

    private bool isMutating = false;
    private float mutationTimer = 0f;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        direction = Random.onUnitSphere;
        baseScale = transform.localScale;
        currentEnergy = maxEnergy;

        rb.useGravity = false;
        rb.drag = waterDrag;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        rend = GetComponentInChildren<Renderer>();
        if (rend != null) originalColor = rend.material.color;
    }

    void FixedUpdate()
    {
        if (isMutating)
        {
            // Petit flottement
            rb.MovePosition(rb.position + Vector3.up * mutationFloatSpeed * Time.fixedDeltaTime);
            return;
        }

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (isMutating)
        {
            mutationTimer += Time.deltaTime;

            if (rend != null)
            {
                float flicker = Mathf.Sin(Time.time * mutationFlashSpeed) * 0.5f + 0.5f;
                rend.material.color = Color.Lerp(originalColor, Color.white, flicker);
            }

            return;
        }

        Wander();
        Pulse();
        DrainEnergy();

        if (currentEnergy <= 0f)
        {
            StartMutation();
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

    void StartMutation()
    {
        if (possibleMutations.Length == 0) return;

        isMutating = true;
        rb.velocity = Vector3.zero;
        Invoke(nameof(CompleteMutation), mutationDelay);
    }

    void CompleteMutation()
    {
        GameObject chosenForm = possibleMutations[Random.Range(0, possibleMutations.Length)];
        Instantiate(chosenForm, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
