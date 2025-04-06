using UnityEngine;
using System.Collections;

public class QuantumTransitionCinematic : MonoBehaviour
{
    public Camera cinematicCam;
    public Transform focusPoint;
    public AudioSource ambientAudio;
    public AudioClip warpSound;
    public GameObject distortionVFX;
    public GameObject quantumWorldScene;
    public float slowDownDuration = 3f;
    public float transitionHoldTime = 2f;
    public float cameraZoomSpeed = 2f;

    private float originalTimeScale = 1f;
    private bool hasStarted = false;

    void Start()
    {
        cinematicCam.enabled = false;
    }

    public void StartQuantumCinematic()
    {
        if (hasStarted) return;
        hasStarted = true;
        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {
        cinematicCam.enabled = true;
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0.2f; // slow time effect

        if (distortionVFX)
            distortionVFX.SetActive(true);

        if (ambientAudio && warpSound)
        {
            ambientAudio.pitch = 0.5f;
            ambientAudio.PlayOneShot(warpSound);
        }

        float t = 0f;
        Vector3 startPos = cinematicCam.transform.position;
        Quaternion startRot = cinematicCam.transform.rotation;
        Vector3 dir = (focusPoint.position - startPos).normalized;

        while (t < slowDownDuration)
        {
            t += Time.unscaledDeltaTime;
            cinematicCam.transform.position = Vector3.Lerp(startPos, focusPoint.position, t / slowDownDuration);
            cinematicCam.transform.LookAt(focusPoint);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(transitionHoldTime);

        Time.timeScale = originalTimeScale;
        SwitchToQuantumWorld();
    }

    void SwitchToQuantumWorld()
    {
        if (quantumWorldScene)
        {
            quantumWorldScene.SetActive(true);
        }

        // Fade out or disable current world if needed
        cinematicCam.enabled = false;
        distortionVFX.SetActive(false);
    }
}
