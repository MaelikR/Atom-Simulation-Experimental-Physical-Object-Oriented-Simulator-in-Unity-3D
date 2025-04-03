// FishGenetics.cs
using UnityEngine;

public class FishGenetics : MonoBehaviour
{
    public FishDNA dna;
    public Renderer fishRenderer;
#if UNITY_EDITOR
void OnValidate()
{
    if (dna == null)
    {
        dna = FishDNA.GenerateRandom();

        ApplyDNA();
    }
}
#endif

    void Start()
    {
        if (dna == null)
            dna = FishDNA.GenerateRandom();

        ApplyDNA();
    }

    void ApplyDNA()
    {
        if (fishRenderer)
            fishRenderer.material.color = dna.bodyColor;
    }

    public FishDNA GetMutatedOffspring()
    {
        return FishDNA.Mutate(dna);
    }
}
