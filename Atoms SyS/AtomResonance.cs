using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class AtomResonance : MonoBehaviour
{
    public Transform atomCenter; // L'atome maître
    public Color glowColor = Color.white;
    public float maxDistance = 3f;
    public float maxIntensity = 3f;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (atomCenter == null) return;

        float dist = Vector3.Distance(transform.position, atomCenter.position);
        float t = Mathf.Clamp01(1f - dist / maxDistance);
        float intensity = t * maxIntensity;
        mat.SetColor("_EmissionColor", glowColor * intensity);
    }
}
