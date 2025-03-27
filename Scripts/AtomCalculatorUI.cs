using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Fusion;

public class AtomCalculatorUI : NetworkBehaviour
{
	[SerializeField] private TMP_Text resultText;
	[SerializeField] private Slider diameterSlider;
	[SerializeField] private Slider heightSlider;

	private AtomCalculatorNetwork calculatorNetwork;

	public override void Spawned()
	{
		calculatorNetwork = FindObjectOfType<AtomCalculatorNetwork>();

		if (HasInputAuthority)
		{
			diameterSlider.onValueChanged.AddListener(OnSliderChanged);
			heightSlider.onValueChanged.AddListener(OnSliderChanged);
			UpdateUI(); // Init
		}
		else
		{
			diameterSlider.interactable = false;
			heightSlider.interactable = false;
		}
	}

	private void OnSliderChanged(float _)
	{
		if (calculatorNetwork != null && HasInputAuthority)
		{
			calculatorNetwork.RPC_UpdateValues(diameterSlider.value, heightSlider.value);
		}
	}

	void Update()
	{
		if (calculatorNetwork != null)
		{
			float totalAtoms = calculatorNetwork.TotalAtoms;
			resultText.text = $"Atomes : {totalAtoms:E2}";
		}
	}

	private void UpdateUI()
	{
		diameterSlider.value = calculatorNetwork.Diameter;
		heightSlider.value = calculatorNetwork.Height;
	}
}
