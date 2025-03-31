using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class JellyfishBehavior : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public float driftSpeed = 0.3f;
    public float rotationSpeed = 20f;

    [Header("Vertical Limits")]
    public float waterSurfaceY = -90f;
    public float maxDepth = 10f;

    [Header("Bioluminescence")]
    public Color dayColor = Color.black;
    public Color nightColor = new Color(0.5f, 1f, 1f);
    public float pulseSpeed = 2f;

    private Material mat;
    private Vector3 initialPosition;
    private float timeOffset;

    void Start()
    {
        initialPosition = transform.position;
        timeOffset = Random.Range(0f, 100f); // Pour variation naturelle
        mat = GetComponent<Renderer>().material;
        SetGlowColor(0f); // Initial off
    }

    void Update()
    {
        // Flottement vertical (oscillation sinusoïdale)
        float vertical = Mathf.Sin((Time.time + timeOffset) * floatFrequency) * floatAmplitude;

        // Clamp la hauteur entre surface et profondeur
        float minY = waterSurfaceY - maxDepth;
        float targetY = Mathf.Clamp(initialPosition.y + vertical, minY, waterSurfaceY);

        // Déplacement fluide
        Vector3 nextPos = transform.position;
        nextPos.y = targetY;
        nextPos += transform.forward * driftSpeed * Time.deltaTime;
        transform.position = nextPos;

        // Rotation douce
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Glow
        float time = DayNightCycle.CurrentTimeOfDay;
        float intensity = (time < 0.2f || time > 0.8f) ? Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) : 0f;
        SetGlowColor(intensity);
    }

    void SetGlowColor(float intensity)
    {
        Color emission = Color.Lerp(dayColor, nightColor, intensity);
        mat.SetColor("_EmissionColor", emission);
        DynamicGI.SetEmissive(GetComponent<Renderer>(), emission);
    }
}
