public class Molecule
{
    public string Name;
    public int AtomCount;
    public float MolarMass; // g/mol
    public bool IsStable;
    public string Formula;  // exemple : "H2O", "C6H12O6"

    public Molecule(string name, int atomCount, float molarMass, bool isStable, string formula)
    {
        Name = name;
        AtomCount = atomCount;
        MolarMass = molarMass;
        IsStable = isStable;
        Formula = formula;
    }
}
