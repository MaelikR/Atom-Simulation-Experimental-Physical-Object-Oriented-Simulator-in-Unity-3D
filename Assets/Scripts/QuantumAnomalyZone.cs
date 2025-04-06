// QuantumAnomalyZone.cs
using UnityEngine;

public class QuantumAnomalyZone : MonoBehaviour
{
    public float baseTimeScale = 0.5f; // Temps ralenti (ou > 1 pour acc�l�r�)
    public AnimationCurve timeDistortionByMass; // 0..1 selon masse atomique normalis�e
    public AudioLowPassFilter distortionFilterPrefab;

    private void OnTriggerEnter(Collider other)
    {
        LivingOrganism organism = other.GetComponent<LivingOrganism>();
        if (organism != null)
        {
            float atomicMass = organism.GetAtomicMass(); // doit �tre normalis�
            float factor = timeDistortionByMass.Evaluate(atomicMass / 100f); // suppose masse entre 0 et 100
            organism.SetLocalTimeScale(baseTimeScale * factor);

            // Ajout filtre audio
            AudioSource source = other.GetComponent<AudioSource>();
            if (source && distortionFilterPrefab)
            {
                var filter = other.gameObject.AddComponent<AudioLowPassFilter>();
                filter.cutoffFrequency = 500f; // effet �touff�
                filter.lowpassResonanceQ = 1f;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        LivingOrganism organism = other.GetComponent<LivingOrganism>();
        if (organism != null)
        {
            organism.SetLocalTimeScale(1f); // Reset normal

            // Supprimer le filtre s�il existe
            var filter = other.GetComponent<AudioLowPassFilter>();
            if (filter)
                Destroy(filter);
        }
    }
}
