public class LiquidBody
{
    public float Diameter; // m
    public float Height;   // m
    public float Density;  // kg/m³
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
        double volume = Mathf.PI * Mathf.Pow((float)radius, 2) * Height;
        double mass = volume * Density; // kg
        double grams = mass * 1000;

        double moles = grams / MoleculeType.MolarMass;
        double molecules = moles * 6.022e23;

        return molecules * MoleculeType.AtomCount;
    }
}
