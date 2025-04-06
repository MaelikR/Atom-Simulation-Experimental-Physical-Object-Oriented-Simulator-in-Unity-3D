using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuantumUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Image energyCircle;
    public Gradient energyGradient;
    public float maxEnergy = 100f;
    public float currentEnergy = 100f;

    public Image stabilityHex;
    public Color stableColor = Color.cyan;
    public Color unstableColor = Color.red;

    public RectTransform flyCompass;
    public Transform player;

    public TMP_Text modeText;
    public Animator uiAnimator;

    private float energyPulseTime;

    void Update()
    {
        UpdateEnergyUI();
        UpdateStabilityUI();
        UpdateCompass();
        UpdateMode();
    }

    void UpdateEnergyUI()
    {
        if (energyCircle == null) return;
        float fill = Mathf.Clamp01(currentEnergy / maxEnergy);
        energyCircle.fillAmount = fill;
        energyCircle.color = energyGradient.Evaluate(fill);

        // Pulse effect
        float pulse = 1f + Mathf.Sin(Time.time * 5f) * 0.05f;
        energyCircle.transform.localScale = Vector3.one * pulse;
    }

    void UpdateStabilityUI()
    {
        if (stabilityHex == null) return;
        bool isStable = currentEnergy >= 30f;
        stabilityHex.color = isStable ? stableColor : unstableColor;

        // Optional: Animate shader or distortion if unstable
        if (!isStable && uiAnimator)
        {
            uiAnimator.SetTrigger("Unstable");
        }
    }

    void UpdateCompass()
    {
        if (flyCompass == null || player == null) return;
        float yRot = player.eulerAngles.y;
        flyCompass.localEulerAngles = new Vector3(0, 0, -yRot);
    }

    void UpdateMode()
    {
        if (modeText == null) return;
        string mode = "Humain";
        if (Input.GetKey(KeyCode.V)) mode = "Vol Libre";
        if (Input.GetKey(KeyCode.B)) mode = "Quantum Bomb";

        modeText.text = $"Mode : <color=#00FFF7>{mode}</color>";
    }
}
