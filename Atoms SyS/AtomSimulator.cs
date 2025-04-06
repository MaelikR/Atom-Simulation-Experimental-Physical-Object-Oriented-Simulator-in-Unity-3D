using TMPro;
using UnityEngine;

public class AtomSimulator : MonoBehaviour
{
    public TMP_Text atomCountText;
    public LiquidBody body;

    void Start()
    {
        var water = new Molecule("H2O", 3, 18.015f, true, "H₂O");

        body = new LiquidBody(0.3f, 0.005f, 1000f, water);
        DisplayAtoms();
    }

    void DisplayAtoms()
    {
        double atoms = body.GetTotalAtoms();
        atomCountText.text = $"Atomes dans cette eau : {atoms:E2}";
    }
}
