public class Molecule
{
    public string Name;
    public int AtomCount;
    public float MolarMass; // en g/mol

    public Molecule(string name, int atomCount, float molarMass)
    {
        Name = name;
        AtomCount = atomCount;
        MolarMass = molarMass;
    }
}
