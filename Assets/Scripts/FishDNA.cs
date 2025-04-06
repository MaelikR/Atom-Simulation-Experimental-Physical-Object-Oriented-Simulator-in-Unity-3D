using UnityEngine;

[System.Serializable]
public class FishDNA
{
    [Header("Movement Genes")]
    public float swimSpeed;
    public float turnSpeed;

    [Header("Behavior Genes")]
    public float curiosity;
    public float fleeDistance;

    [Header("Appearance Genes")]
    public Color bodyColor;

    [Header("Survival Genes")]
    public float energyEfficiency; // Lower drain = better

    public static FishDNA GenerateRandom()
    {
        return new FishDNA
        {
            swimSpeed = Random.Range(1.5f, 4f),
            turnSpeed = Random.Range(1f, 3f),
            curiosity = Random.Range(0.5f, 2f),
            fleeDistance = Random.Range(3f, 7f),
            energyEfficiency = Random.Range(0.005f, 0.02f),
            bodyColor = new Color(Random.value, Random.value, Random.value)
        };
    }

    public static FishDNA Mutate(FishDNA parent)
    {
        FishDNA mutated = new FishDNA();

        mutated.swimSpeed = Mathf.Clamp(parent.swimSpeed + Random.Range(-0.3f, 0.3f), 1f, 5f);
        mutated.turnSpeed = Mathf.Clamp(parent.turnSpeed + Random.Range(-0.2f, 0.2f), 0.5f, 5f);
        mutated.curiosity = Mathf.Clamp(parent.curiosity + Random.Range(-0.3f, 0.3f), 0.1f, 3f);
        mutated.fleeDistance = Mathf.Clamp(parent.fleeDistance + Random.Range(-1f, 1f), 2f, 10f);
        mutated.energyEfficiency = Mathf.Clamp(parent.energyEfficiency + Random.Range(-0.002f, 0.002f), 0.001f, 0.03f);

        mutated.bodyColor = new Color(
            Mathf.Clamp01(parent.bodyColor.r + Random.Range(-0.1f, 0.1f)),
            Mathf.Clamp01(parent.bodyColor.g + Random.Range(-0.1f, 0.1f)),
            Mathf.Clamp01(parent.bodyColor.b + Random.Range(-0.1f, 0.1f))
        );

        return mutated;
    }
}
