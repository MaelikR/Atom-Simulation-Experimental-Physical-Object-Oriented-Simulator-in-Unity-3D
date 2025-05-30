using UnityEngine;
using System.Collections.Generic;

public class SharkAtomSystem : MonoBehaviour
{
    public List<Molecule> composition = new List<Molecule>();

    void Start()
    {
        composition.Add(new Molecule("Water (H2O)", 3, 18.015f));
        composition.Add(new Molecule("Protein", 20000, 50000f));
        composition.Add(new Molecule("Calcium", 20, 40.078f)); // pour les os/cartilage
    }

    public float GetTotalMolarMass()
    {
        float total = 0f;
        foreach (var molecule in composition)
        {
            total += molecule.MolarMass * molecule.AtomCount;
        }
        return total;
    }

    public void PrintComposition()
    {
        Debug.Log($"--- Molecular Composition of {gameObject.name} ---");
        foreach (var mol in composition)
        {
            Debug.Log($"{mol.Name} Atoms: {mol.AtomCount} | Molar Mass: {mol.MolarMass}");
        }
        Debug.Log($"Total Mass (approx): {GetTotalMolarMass()} g/mol");
    }
}
