using UnityEngine;

public class BioluminescentOrganism : MonoBehaviour
{
    [Header("Glow Settings")]
    public Renderer glowRenderer;
    public Color dayColor = Color.black;
    public Color nightColor = new Color(0.2f, 0.9f, 1f); // Bleu néon
    public float transitionSpeed = 2f;

    private Material _mat;
    private float targetEmission = 0f;

    void Start()
    {
        if (glowRenderer == null)
            glowRenderer = GetComponent<Renderer>();

        _mat = glowRenderer.material;
        UpdateGlowColor(0f); // Initialiser avec aucun glow
    }

    void Update()
    {
        float t = DayNightCycle.CurrentTimeOfDay;
        float glowTarget = (t < 0.2f || t > 0.8f) ? 1f : 0f;

        targetEmission = Mathf.Lerp(targetEmission, glowTarget, Time.deltaTime * transitionSpeed);
        UpdateGlowColor(targetEmission);
    }

    void UpdateGlowColor(float intensity)
    {
        Color baseColor = Color.Lerp(dayColor, nightColor, intensity);
        _mat.SetColor("_EmissionColor", baseColor);
        DynamicGI.SetEmissive(glowRenderer, baseColor);
    }
}
