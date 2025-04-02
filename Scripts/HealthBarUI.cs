using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
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

    public void Setup(Transform targetTransform, float maxValue)
    {
        target = targetTransform;
        slider.maxValue = maxValue;
        slider.value = maxValue;
        UpdateColor(1f);
    }

    public void UpdateHealth(float current)
    {
        slider.value = current;
        float ratio = current / slider.maxValue;
        UpdateColor(ratio);
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

    // Alias rétro-compatible
    public void UpdateValue(float current)
    {
        UpdateHealth(current);
    }

    void LateUpdate()
    {
        if (target == null || Camera.main == null) return;

        transform.position = target.position + offset;
        transform.LookAt(Camera.main.transform);
    }
}
