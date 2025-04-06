
using UnityEngine;

public class MagicHandEffect : MonoBehaviour
{
    public ParticleSystem magicParticles;
    public Light magicLight;

    [Header("Settings")]
    public Color effectColor = Color.cyan;
    public float pulseSpeed = 2f;
    public float lightIntensity = 3f;

    private float pulseTimer;

    void Start()
    {
        if (magicParticles != null)
        {
            var main = magicParticles.main;
            main.startColor = effectColor;
        }

        if (magicLight != null)
        {
            magicLight.color = effectColor;
            magicLight.intensity = lightIntensity;
        }
    }

    void Update()
    {
        if (magicLight != null)
        {
            pulseTimer += Time.deltaTime * pulseSpeed;
            magicLight.intensity = lightIntensity + Mathf.Sin(pulseTimer) * 0.5f;
        }
    }

    public void EnableEffect(bool enable)
    {
        if (magicParticles != null)
        {
            if (enable && !magicParticles.isPlaying) magicParticles.Play();
            else if (!enable && magicParticles.isPlaying) magicParticles.Stop();
        }

        if (magicLight != null)
            magicLight.enabled = enable;
    }
}
