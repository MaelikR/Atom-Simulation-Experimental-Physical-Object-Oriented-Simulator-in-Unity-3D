using UnityEngine;

public class AtomCalculator : MonoBehaviour
{
    void Start()
    {
        // Dimensions de la flaque (mètres)
        float diameter = 0.3f; // 30 cm → 0.3 m
        float height = 0.005f; // 0.5 cm → 0.005 m

        // Constantes
        float densityWater = 1000f; // kg/m³ (masse volumique de l'eau)
        float molarMassWater = 18.015f; // g/mol (masse molaire de H₂O)
        float avogadroNumber = 6.022e23f; // Nombre d'Avogadro (molécules par mole)

        // Calcul du volume de la flaque (V = π * r² * h)
        float radius = diameter / 2f;
        float volume = Mathf.PI * Mathf.Pow(radius, 2) * height; // en m³

        // Calcul de la masse de l'eau (masse = volume * densité)
        float mass = volume * densityWater; // en kg
        float massInGrams = mass * 1000f; // Conversion en grammes

        // Nombre de moles d'eau
        float molesOfWater = massInGrams / molarMassWater;

        // Nombre de molécules d'eau
        float numberOfMolecules = molesOfWater * avogadroNumber;

        // Nombre total d'atomes (H₂O = 3 atomes par molécule)
        float totalAtoms = numberOfMolecules * 3;

        // Affichage du résultat
        Debug.Log($"Nombre total d'atomes dans la flaque d'eau : {totalAtoms:E2}");
    }
}
