using UnityEngine;

public class CosmicDustSpawner : MonoBehaviour
{
    [Header("Particle Settings")]
    public int particleCount = 50;
    public GameObject dustPrefab; // Assign a simple transparent sprite or mesh
    public float spawnRadius = 3f;
    public float floatSpeed = 0.1f;
    public float driftRange = 0.05f;
    public float scaleMin = 0.1f;
    public float scaleMax = 0.3f;

    void Start()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spawnRadius;
            GameObject dust = Instantiate(dustPrefab, randomPos, Quaternion.identity, transform);

            float randomScale = Random.Range(scaleMin, scaleMax);
            dust.transform.localScale = Vector3.one * randomScale;

            // Start floating behavior
            StartCoroutine(FloatParticle(dust.transform));
        }
    }

    System.Collections.IEnumerator FloatParticle(Transform particle)
    {
        Vector3 startPos = particle.position;
        float offset = Random.Range(0f, 2f * Mathf.PI);

        while (true)
        {
            float t = Time.time * floatSpeed + offset;
            Vector3 drift = new Vector3(
                Mathf.Sin(t) * driftRange,
                Mathf.Cos(t) * driftRange * 0.5f,
                Mathf.Sin(t * 0.5f) * driftRange
            );
            particle.position = startPos + drift;
            yield return null;
        }
    }
}
