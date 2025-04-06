using UnityEngine;
using System.Collections;

public class AtomIntroLogicSystem : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject player;
    public GameObject quantumFieldVFX;
    public GameObject distortionSphere;
    public AudioSource voiceSynth;
    public AudioClip[] introClips;
    public GameObject cosmicHUD;
    public Animator cameraAnimator;

    [Header("Timing Settings")]
    public float delayBeforeIntro = 2f;
    public float fieldActivationDelay = 3f;

    private bool introLaunched = false;

    void Start()
    {
        if (!introLaunched)
        {
            StartCoroutine(LaunchIntroSequence());
        }
    }

    IEnumerator LaunchIntroSequence()
    {
        introLaunched = true;

        yield return new WaitForSeconds(delayBeforeIntro);

        if (quantumFieldVFX != null)
            quantumFieldVFX.SetActive(true);

        if (distortionSphere != null)
            distortionSphere.SetActive(true);

        if (voiceSynth && introClips.Length > 0)
        {
            yield return new WaitForSeconds(1f);
            voiceSynth.PlayOneShot(introClips[0]);
        }

        yield return new WaitForSeconds(fieldActivationDelay);

        if (cosmicHUD != null)
            cosmicHUD.SetActive(true);

        if (cameraAnimator != null)
            cameraAnimator.SetTrigger("IntroZoom");
    }

    // Peut être appelée après l’intro pour déclencher la suite
    public void TriggerAtomRevealSequence()
    {
        // Ajouter ici la suite (spawn particules, narration, activer portails, etc)
        Debug.Log("[AtomIntroLogicSystem] Atom reveal started.");
    }
}
