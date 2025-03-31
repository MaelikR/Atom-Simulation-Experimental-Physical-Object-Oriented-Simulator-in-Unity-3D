using UnityEngine;
using System.Collections.Generic;

public class JellyfishBehavior : MonoBehaviour
{
    [Header("Movement")]
    public float floatSpeed = 0.5f;
    public float verticalOscillationSpeed = 2f;
    public float verticalOscillationHeight = 0.2f;

    [Header("Reproduction")]
    public GameObject jellyfishPrefab;
    public float reproductionCooldown = 15f;
    public float detectionRadius = 3f;
    public float spawnOffset = 1f;

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
        Collider[] others = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var col in others)
        {
            if (col.gameObject != this.gameObject && col.GetComponent<JellyfishBehavior>())
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnOffset;
                spawnPos.y = Mathf.Clamp(spawnPos.y, -100f, 0f); // pour ne pas sortir de l'eau
                Instantiate(jellyfishPrefab, spawnPos, Quaternion.identity);
                break;
            }
        }
    }
}
