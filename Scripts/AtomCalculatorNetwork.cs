using UnityEngine;
using Fusion;

public class AtomCalculatorNetwork : NetworkBehaviour
{
    [Networked] public float diameter { get; set; }
    [Networked] public float height { get; set; }

    void Start()
    {
        if (Object.HasInputAuthority)
        {
            diameter = 0.3f;
            height = 0.005f;
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority)
        {
            float totalAtoms = CalculateAtoms(diameter, height);
            Debug.Log($"(Network) Nombre total d'atomes : {totalAtoms:E2}");
        }
    }

    float CalculateAtoms(float diameter, float height)
    {
        float densityWater = 1000f;
        float molarMassWater = 18.015f;
        float avogadroNumber = 6.022e23f;

        float radius = diameter / 2f;
        float volume = Mathf.PI * Mathf.Pow(radius, 2) * height;
        float mass = volume * densityWater;
        float massInGrams = mass * 1000f;

        float molesOfWater = massInGrams / molarMassWater;
        float numberOfMolecules = molesOfWater * avogadroNumber;
        return numberOfMolecules * 3;
    }
}
