using UnityEngine;
using Fusion;

public class AtomCalculatorNetwork : NetworkBehaviour
{
    [Networked] public float diameter { get; set; }
    [Networked] public float height { get; set; }
    [Networked] public float TotalAtoms { get; set; }

    public override void Spawned()
    {
        if (HasInputAuthority)
        {
            diameter = 0.3f;
            height = 0.005f;
            TotalAtoms = CalculateAtoms(diameter, height);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_UpdateValues(float newDiameter, float newHeight)
    {
        diameter = newDiameter;
        height = newHeight;
        TotalAtoms = CalculateAtoms(diameter, height);
    }

    private float CalculateAtoms(float d, float h)
    {
        float densityWater = 1000f;
        float molarMassWater = 18.015f;
        float avogadroNumber = 6.022e23f;

        float radius = d / 2f;
        float volume = Mathf.PI * Mathf.Pow(radius, 2) * h;
        float mass = volume * densityWater;
        float massInGrams = mass * 1000f;

        float molesOfWater = massInGrams / molarMassWater;
        float numberOfMolecules = molesOfWater * avogadroNumber;
        return numberOfMolecules * 3;
    }
}
