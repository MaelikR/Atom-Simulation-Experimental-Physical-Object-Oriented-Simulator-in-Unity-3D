using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 2f, 0);
    private Transform target;

    public void Setup(Transform targetTransform, float maxValue)
    {
        target = targetTransform;
        if (slider != null)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
        else
        {
            Debug.LogError("Slider is not assigned on " + gameObject.name);
        }
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
        transform.forward = Camera.main.transform.forward;
    }
}
