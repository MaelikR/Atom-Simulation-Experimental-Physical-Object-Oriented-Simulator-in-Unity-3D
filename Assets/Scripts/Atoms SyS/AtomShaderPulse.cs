using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AtomShaderPulse : MonoBehaviour
{
    public float baseScale = 1f;
    public float pulseSpeed = 2f;
    public float pulseAmplitude = 0.2f;
    public Color emissionColor = Color.cyan;
    public float emissionIntensity = 2f;
    public bool enableFission = true;
    public float fissionChancePerSecond = 0.001f;

    private Renderer rend;
    private Material mat;
    private float seed;
    private AtomFission fission;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material;
        seed = Random.Range(0f, 100f);
        fission = GetComponent<AtomFission>();

        if (mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    void Update()
    {
        // Pulsation sinusoïdale
        float scale = baseScale + Mathf.Sin(Time.time * pulseSpeed + seed) * pulseAmplitude;
        transform.localScale = Vector3.one * scale;

        // Emission dynamique
        if (mat.HasProperty("_EmissionColor"))
        {
            float intensity = (Mathf.Sin(Time.time * pulseSpeed + seed) * 0.5f + 0.5f) * emissionIntensity;
            Color finalEmission = emissionColor * intensity;
            mat.SetColor("_EmissionColor", finalEmission);
        }

        // Fission aléatoire
        if (enableFission && fission != null && Random.value < fissionChancePerSecond * Time.deltaTime)
        {
            fission.TriggerFission();
        }
    }
}
