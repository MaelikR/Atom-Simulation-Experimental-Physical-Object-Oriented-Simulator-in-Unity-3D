using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider slider;

    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform target;

    public void Setup(Transform targetTransform, float maxValue)
    {
        target = targetTransform;

        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
            if (slider == null)
            {
                Debug.LogError("Aucun Slider trouvé dans " + gameObject.name);
                return;
            }
        }

        slider.maxValue = maxValue;
        slider.value = maxValue;
    }

    public void UpdateValue(float current)
    {
        if (slider != null)
            slider.value = current;
    }

    void LateUpdate()
    {
        if (target == null || Camera.main == null) return;

        transform.position = target.position + offset;
        transform.LookAt(Camera.main.transform);
    }
}
