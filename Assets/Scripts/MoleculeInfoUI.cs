using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;

public class MoleculeInfoUI : MonoBehaviour
{
    public Text nameText;
    public Text stateText;
    public Text formulaText;

    public void Display(Molecule molecule)
    {
        nameText.text = "Nom : " + molecule.Name;
        stateText.text = "État : " + (molecule.IsStable ? "Stable" : "<color=red>Instable</color>");
        formulaText.text = "Formule : " + molecule.Formula;
    }
}
