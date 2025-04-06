using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnderwaterEnvironmentEffects : MonoBehaviour
{
    [Header("Pressure Settings")]
    public float basePressure = 1f;
    public float pressurePerMeter = 0.1f;

    [Header("Current Settings")]
    public Vector3 currentDirection = new Vector3(1f, 0f, 0f);
    public float currentStrength = 2f;

    [Header("Fog Settings")]
    public Color fogColor = new Color(0f, 0.2f, 0.4f, 1f);
    public float fogDensity = 0.05f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            // Apply constant force as underwater current
            rb.AddForce(currentDirection.normalized * currentStrength, ForceMode.Acceleration);
        }

        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            float depth = Mathf.Abs(other.transform.position.y);
            float pressure = basePressure + depth * pressurePerMeter;

            // Simulate pressure: optional effect like increased drag
            rb.drag = Mathf.Clamp(pressure * 0.5f, 1f, 10f);

            // Re-apply current
            rb.AddForce(currentDirection.normalized * currentStrength, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.drag = 1f; // reset drag
        }

        RenderSettings.fog = false;
    }
}
