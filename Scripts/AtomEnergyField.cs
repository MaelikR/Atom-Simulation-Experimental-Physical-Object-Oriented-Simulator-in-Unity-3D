using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AtomEnergyField : MonoBehaviour
{
    [Header("Cosmic Energy Settings")]
    public float energy = 0f;
    public float maxEnergy = 100f;
    public float accumulationRate = 5f;
    public float drainPerOrganism = 2f;

    [Header("Pulse Visuals")]
    public Renderer energyRenderer;
    public Color minColor = new Color(0.2f, 0.2f, 0.6f);
    public Color maxColor = new Color(1f, 0.7f, 0.2f);
    public float pulseSpeed = 2f;
    public float baseIntensity = 1f;

    private SphereCollider detectionZone;

    void Start()
    {
        detectionZone = GetComponent<SphereCollider>();
        detectionZone.isTrigger = true;

        if (energyRenderer == null)
        {
            energyRenderer = GetComponentInChildren<Renderer>();
        }
    }

    void Update()
    {
        AccumulateEnergy();
        PulseVisual();
    }

    void AccumulateEnergy()
    {
        int nearbyOrganisms = CountNearbyOrganisms();
        float totalDrain = drainPerOrganism * nearbyOrganisms;

        energy += (accumulationRate - totalDrain) * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0f, maxEnergy);
    }

    int CountNearbyOrganisms()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionZone.radius);
        int count = 0;
        foreach (Collider c in hits)
        {
            if (c.GetComponent<LivingOrganism>())
                count++;
        }
        return count;
    }

    void PulseVisual()
    {
        if (energyRenderer == null) return;

        float pulse = Mathf.Sin(Time.time * pulseSpeed) * 0.5f + 0.5f;
        float t = energy / maxEnergy;
        Color currentColor = Color.Lerp(minColor, maxColor, t);
        float emissive = baseIntensity + pulse * t;

        energyRenderer.material.SetColor("_EmissionColor", currentColor * emissive);
        DynamicGI.SetEmissive(energyRenderer, currentColor * emissive);
    }

    public float GetCurrentEnergy()
    {
        return energy;
    }
}
