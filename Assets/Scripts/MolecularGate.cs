using UnityEngine;

public class MolecularGate : MonoBehaviour
{
    public string requiredFormula = "C6H12O6";

    public void Check(Molecule mol)
    {
        if (mol.Formula == requiredFormula)
        {
            ActivatePortal();
        }
    }

    void ActivatePortal()
    {
        // FX + activation
        gameObject.SetActive(false); // Exemple
        Debug.Log("🔓 Portail moléculaire activé !");
    }
}
