using UnityEngine;

[System.Serializable]
public class LiquidBody
{
    public float Diameter; // in meters
    public float Height;   // in meters
    public float Density;  // in kg/m^3
    public Molecule MoleculeType;

    public LiquidBody(float diameter, float height, float density, Molecule molecule)
    {
        Diameter = diameter;
        Height = height;
        Density = density;
        MoleculeType = molecule;
    }

    public double GetTotalAtoms()
    {
        double radius = Diameter / 2.0;
        double volume = Mathf.PI * Mathf.Pow((float)radius, 2) * Height; // in m^3
        double mass = volume * Density; // in kg
        double grams = mass * 1000;

        double moles = grams / MoleculeType.MolarMass;
        double molecules = moles * 6.022e23;

        return molecules * MoleculeType.AtomCount;
    }
}
