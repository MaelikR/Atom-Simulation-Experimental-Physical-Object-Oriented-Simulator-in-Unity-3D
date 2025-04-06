using UnityEngine;

public class PhosphoreAtom : MonoBehaviour
{
    [Header("Phosphore Settings")]
    public float lightPulseSpeed = 2f;
    public float lightRange = 3f;
    public Color glowColor = Color.magenta;

    private Light atomLight;
    private Renderer rend;
    private Material mat;
    private float baseIntensity;

    void Start()
    {
        atomLight = GetComponentInChildren<Light>();
        rend = GetComponent<Renderer>();
        mat = rend.material;

        if (atomLight != null)
            baseIntensity = atomLight.intensity;
    }

    void Update()
    {
        float pulse = Mathf.Sin(Time.time * lightPulseSpeed) * 0.5f + 0.5f;

        if (atomLight != null)
            atomLight.intensity = baseIntensity * (0.5f + pulse);

        if (mat != null)
            mat.SetColor("_EmissionColor", glowColor * pulse);
    }
}
