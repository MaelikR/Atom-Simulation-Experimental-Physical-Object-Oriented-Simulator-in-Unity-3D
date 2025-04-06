using UnityEngine;
using System.Collections;

public class IntroTriggerZone : MonoBehaviour
{
    [Header("Ruins Intro Settings")]
    public GameObject cinematicCamera;
    public float cinematicDuration = 8f;
    public GameObject[] objectsToEnableAfter;
    public AudioClip introMusic;

    [Header("Player Settings")]
    public bool disableInput = true;
    public bool lockCursor = false;

    [Header("Environment")]
    public Animator ruinAnimator;
    public string openAnimTrigger = "Open";

    [Header("Optional FX")]
    public ParticleSystem introVFX;
    public GameObject fadeCanvas;
    public bool destroyAfter = true;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(PlayIntroSequence(other.gameObject));
        }
    }

    IEnumerator PlayIntroSequence(GameObject player)
    {
        // Optional: Disable input
        var input = player.GetComponent<FirstPersonCamera>();
        if (input && disableInput)
        {
            input.enabled = false;
        }

        if (fadeCanvas) fadeCanvas.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // Play music
        if (introMusic)
        {
            AudioSource.PlayClipAtPoint(introMusic, transform.position);
        }

        // Play VFX
        if (introVFX) introVFX.Play();

        // Activate cinematic camera
        if (cinematicCamera) cinematicCamera.SetActive(true);

        // Optional: play animation on ruins
        if (ruinAnimator != null)
            ruinAnimator.SetTrigger(openAnimTrigger);

        yield return new WaitForSeconds(cinematicDuration);

        // End cinematic
        if (cinematicCamera) cinematicCamera.SetActive(false);
        if (fadeCanvas) fadeCanvas.SetActive(false);

        // Enable game content
        foreach (var obj in objectsToEnableAfter)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Reactivate input
        if (input && disableInput)
        {
            input.enabled = true;
        }

        if (destroyAfter) Destroy(gameObject);
    }
}
