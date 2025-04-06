using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

using System.Collections.Generic;

public class EnvironmentMutator : MonoBehaviour
{
    [Header("Player Behavior Influence")]
    public float helpThreshold = 5f;
    public float harmThreshold = -5f;

    [Header("Mutation Effects")]
    public Volume postProcessVolume;
    public Light directionalLight;
    public AudioSource ambientAudio;
    public AudioClip peacefulAmbience;
    public AudioClip decayedAmbience;

    [Header("Visual Themes")]
    public Material lushSkybox;
    public Material decayedSkybox;
    public Color lushAmbientLight = new Color(0.7f, 0.9f, 0.7f);
    public Color decayedAmbientLight = new Color(0.2f, 0.2f, 0.25f);

    [Header("Affected Portals")]
    public List<GameObject> portalsToLock;

    private float behaviorScore = 0f;
    private bool environmentMutated = false;
    public Volume globalVolume;

    private Fog fog;
    private VisualEnvironment visualEnv;
    private int helpLevel = 0;
    private int harmLevel = 0;

    void Start()
    {
        if (globalVolume.profile.TryGet(out fog))
            Debug.Log("Fog ready");

        if (globalVolume.profile.TryGet(out visualEnv))
            Debug.Log("Visual environment ready");
    }

    public void RegisterHelpAction()
    {
        helpLevel++;
        UpdateEnvironment();
    }

    public void RegisterHarmAction()
    {
        harmLevel++;
        UpdateEnvironment();
    }

    void UpdateEnvironment()
    {
        float balance = helpLevel - harmLevel;

        if (fog != null)
        {
            fog.albedo.value = balance > 0 ? Color.green : Color.red;
            fog.meanFreePath.value = Mathf.Lerp(30f, 200f, Mathf.InverseLerp(-5f, 5f, balance));
        }

        // Ex : adaptation lumière ou sons à ajouter ici
    }
    

    void CheckMutationState()
    {
        if (!environmentMutated)
        {
            if (behaviorScore >= helpThreshold)
            {
                ApplyLushMutation();
                environmentMutated = true;
            }
            else if (behaviorScore <= harmThreshold)
            {
                ApplyDecayMutation();
                environmentMutated = true;
            }
        }
    }

    void ApplyLushMutation()
    {
        RenderSettings.skybox = lushSkybox;
        RenderSettings.ambientLight = lushAmbientLight;
        if (postProcessVolume != null) postProcessVolume.weight = 0.2f;

        if (directionalLight != null)
        {
            directionalLight.color = Color.yellow;
            directionalLight.intensity = 1.2f;
        }

        if (ambientAudio != null && peacefulAmbience != null)
        {
            ambientAudio.clip = peacefulAmbience;
            ambientAudio.Play();
        }
    }

    void ApplyDecayMutation()
    {
        RenderSettings.skybox = decayedSkybox;
        RenderSettings.ambientLight = decayedAmbientLight;
        if (postProcessVolume != null) postProcessVolume.weight = 0.7f;

        if (directionalLight != null)
        {
            directionalLight.color = Color.gray;
            directionalLight.intensity = 0.4f;
        }

        if (ambientAudio != null && decayedAmbience != null)
        {
            ambientAudio.clip = decayedAmbience;
            ambientAudio.Play();
        }

        foreach (GameObject portal in portalsToLock)
        {
            if (portal != null)
                portal.SetActive(false);
        }
    }
}
