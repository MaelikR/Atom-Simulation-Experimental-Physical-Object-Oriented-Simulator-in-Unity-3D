using UnityEngine;
using System.Collections.Generic;

public class JellyfishBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float floatSpeed = 0.5f;
    public float verticalOscillationSpeed = 2f;
    public float verticalOscillationHeight = 0.2f;

    [Header("Reproduction Settings")]
    public GameObject jellyfishPrefab;
    public float reproductionCooldown = 10f;
    public float reproductionRadius = 3f;
    public int maxPopulation = 50;
    public float detectionRadius = 10f;
    public float energy = 5f;
    public float energyNeededToReproduce = 4f;

    [Header("Life Cycle")]
    public float lifespan = 60f;
    private float age = 0f;

    private float lastReproductionTime = 0f;
    private static List<GameObject> allJellies = new List<GameObject>();

    [Header("Communication")]
    public float pulseInterval = 5f;
    public Color communicationColor = new Color(0.8f, 0.3f, 1f);

    private float originalY;
    private float timeSinceLastReproduction;
    private float timeSinceLastPulse;
    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        originalY = transform.position.y;
        timeSinceLastReproduction = Random.Range(0, reproductionCooldown);
        timeSinceLastPulse = Random.Range(0, pulseInterval);

        if (!allJellies.Contains(gameObject))
            allJellies.Add(gameObject);

        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        // Floating
        float y = originalY + Mathf.Sin(Time.time * verticalOscillationSpeed) * verticalOscillationHeight;
        transform.position += new Vector3(0f, (y - transform.position.y) * Time.deltaTime, 0f);

        // Communication pulse
        timeSinceLastPulse += Time.deltaTime;
        if (timeSinceLastPulse >= pulseInterval)
        {
            EmitPulse();
            timeSinceLastPulse = 0f;
        }

        // Lifespan
        age += Time.deltaTime;
        if (age >= lifespan)
        {
            Die();
            return;
        }

        // Reproduction
        timeSinceLastReproduction += Time.deltaTime;
        if (timeSinceLastReproduction >= reproductionCooldown)
        {
            TryReproduce();
            timeSinceLastReproduction = 0f;
        }
    }

    void EmitPulse()
    {
        if (rend != null)
        {
            StartCoroutine(PulseGlow());
        }
    }

    System.Collections.IEnumerator PulseGlow()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            Color pulse = Color.Lerp(originalColor, communicationColor, Mathf.Sin(t * Mathf.PI));
            rend.material.SetColor("_EmissionColor", pulse);
            DynamicGI.SetEmissive(rend, pulse);
            yield return null;
        }
    }

    void TryReproduce()
    {
        if (Time.time - lastReproductionTime < reproductionCooldown) return;
        if (energy < energyNeededToReproduce) return;
        if (allJellies.Count >= maxPopulation) return;

        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius);
        int nearbyCount = 0;

        foreach (var col in nearby)
        {
            if (col.GetComponent<JellyfishBehavior>() != null)
                nearbyCount++;
        }

        if (nearbyCount > 1)
        {
            Vector3 offset = Random.insideUnitSphere * reproductionRadius;
            offset.y = Mathf.Clamp(offset.y, -1f, 1f);

            GameObject child = Instantiate(jellyfishPrefab, transform.position + offset, Quaternion.identity);
            allJellies.Add(child);
            lastReproductionTime = Time.time;
            energy = 0f;
        }
    }

    void Die()
    {
        allJellies.Remove(gameObject);
        Destroy(gameObject);
    }
}
