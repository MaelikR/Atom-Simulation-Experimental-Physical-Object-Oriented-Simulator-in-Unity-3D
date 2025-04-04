using UnityEngine;
using UnityEngine.UI;

public class MoleculeScanUI : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup panelGroup;
    public Text moleculeNameText;
    public Text moleculeStateText;
    public Image stateIcon;

    [Header("Icons")]
    public Sprite chargedIcon;
    public Sprite unstableIcon;
    public Sprite bondedIcon;

    [Header("Fade Settings")]
    public float fadeSpeed = 3f;

    private bool isVisible = false;

    void Update()
    {
        panelGroup.alpha = Mathf.Lerp(panelGroup.alpha, isVisible ? 1 : 0, Time.deltaTime * fadeSpeed);
        panelGroup.blocksRaycasts = isVisible;
    }

    public void Show(MoleculeInfo mol)
    {
        moleculeNameText.text = mol.moleculeName;
        moleculeStateText.text = mol.moleculeState;

        switch (mol.moleculeState)
        {
            case "Charged":      stateIcon.sprite = chargedIcon; break;
            case "Unstable":     stateIcon.sprite = unstableIcon; break;
            case "Bonded":       stateIcon.sprite = bondedIcon; break;
            default:             stateIcon.sprite = null; break;
        }

        isVisible = true;
    }

    public void Hide() => isVisible = false;
}
