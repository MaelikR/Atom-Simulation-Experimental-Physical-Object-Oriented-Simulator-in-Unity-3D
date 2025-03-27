using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AtomCalculatorUI : MonoBehaviour
{
    public TMP_Text resultText;
    public Slider diameterSlider;
    public Slider heightSlider;

    void Start()
    {
        diameterSlider.onValueChanged.AddListener(UpdateCalculation);
        heightSlider.onValueChanged.AddListener(UpdateCalculation);
        UpdateCalculation(0); // Calcul initial
    }

    void UpdateCalculation(float value)
    {
        float diameter = diameterSlider.value;
        float height = heightSlider.value;

        float totalAtoms = CalculateAtoms(diameter, height);
        resultText.text = $"Atomes : {totalAtoms:E2}";
    }

    float CalculateAtoms(float diameter, float height)
    {
        float densityWater = 1000f; // kg/m³
        float molarMassWater = 18.015f; // g/mol
        float avogadroNumber = 6.022e23f;

        float radius = diameter / 2f;
        float volume = Mathf.PI * Mathf.Pow(radius, 2) * height;
        float mass = volume * densityWater;
        float massInGrams = mass * 1000f;

        float molesOfWater = massInGrams / molarMassWater;
        float numberOfMolecules = molesOfWater * avogadroNumber;
        return numberOfMolecules * 3;
    }
}
