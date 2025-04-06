using UnityEngine;

public class CarboneAtom : MonoBehaviour
{
    [Header("Carbone Settings")]
    public float rotationSpeed = 30f;
    public float pulseScale = 0.2f;
    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        float scalePulse = Mathf.Sin(Time.time * 2f) * pulseScale;
        transform.localScale = baseScale + Vector3.one * scalePulse;
    }
}
