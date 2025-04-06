using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class HealthBarUI : NetworkBehaviour
{
    [Header("UI")]
    public Slider slider;
    public Image fillImage;

    [Header("Follow Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);

    [Header("Color Thresholds")]
    public Color highColor = Color.green;
    public Color midColor = new Color(1f, 0.65f, 0f); // orange
    public Color lowColor = Color.red;

    private float targetHealth = 100f;
    private float maxHealth = 100f;

    public void Setup(Transform targetTransform, float maxValue)
    {
        target = targetTransform;
        maxHealth = maxValue;
        targetHealth = maxValue;

        slider.maxValue = maxValue;
        slider.value = maxValue;
        UpdateColor(1f);
    }

    public void UpdateHealth(float current)
    {
        targetHealth = current;
        slider.value = targetHealth;
        UpdateColor(targetHealth / maxHealth);
    }

    void UpdateColor(float ratio)
    {
        if (fillImage == null) return;

        if (ratio > 0.6f)
            fillImage.color = highColor;
        else if (ratio > 0.3f)
            fillImage.color = midColor;
        else
            fillImage.color = lowColor;
    }

    public void UpdateValue(float current) => UpdateHealth(current);

    void LateUpdate()
    {
        if (target == null || Camera.main == null) return;

        transform.position = target.position + offset;
        transform.LookAt(Camera.main.transform);
    }
}
