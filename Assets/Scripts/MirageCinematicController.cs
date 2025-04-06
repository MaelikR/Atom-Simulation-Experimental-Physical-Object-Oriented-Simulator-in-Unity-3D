using UnityEngine;
using System.Collections;

public class MirageCinematicController : MonoBehaviour
{
    public Camera cinematicCamera;
    public Transform player;
    public Transform reflectionOrigin;
    public Transform skyTarget;
    public float travelDuration = 4f;

    public AudioSource ambientAudio;
    public AudioClip transitionSound;
    public AnimationCurve fovCurve;
    public GameObject distortionSphere;
    public Material screenTearMaterial;
    public float timeScaleTarget = 0.3f;
    public float postTransitionDelay = 2f;

    private float originalTimeScale;
    private bool isTransitioning = false;

    void Start()
    {
        originalTimeScale = Time.timeScale;
    }

    public void StartMirageTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(PlayCinematic());
        }
    }

    IEnumerator PlayCinematic()
    {
        isTransitioning = true;
        player.gameObject.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);
        distortionSphere.SetActive(true);

        Vector3 startPos = reflectionOrigin.position;
        Vector3 endPos = skyTarget.position;
        Quaternion startRot = Quaternion.LookRotation(reflectionOrigin.forward);
        Quaternion endRot = Quaternion.LookRotation(skyTarget.forward);

        float startFOV = cinematicCamera.fieldOfView;
        float endFOV = 30f;

        float t = 0f;
        if (ambientAudio && transitionSound)
        {
            ambientAudio.clip = transitionSound;
            ambientAudio.pitch = 0.7f;
            ambientAudio.Play();
        }

        while (t < 1f)
        {
            t += Time.deltaTime / travelDuration;
            float curvedT = Mathf.SmoothStep(0f, 1f, t);

            cinematicCamera.transform.position = Vector3.Lerp(startPos, endPos, curvedT);
            cinematicCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, curvedT);
            cinematicCamera.fieldOfView = Mathf.Lerp(startFOV, endFOV, fovCurve.Evaluate(curvedT));

            yield return null;
        }

        StartCoroutine(ApplyScreenTear());

        yield return new WaitForSecondsRealtime(postTransitionDelay);

        Time.timeScale = timeScaleTarget;
        player.position = skyTarget.position + Vector3.up * 2f;
        player.gameObject.SetActive(true);
        cinematicCamera.gameObject.SetActive(false);
        distortionSphere.SetActive(false);

        isTransitioning = false;
    }

    IEnumerator ApplyScreenTear()
    {
        if (screenTearMaterial != null)
        {
            float tear = 0f;
            while (tear < 1f)
            {
                tear += Time.unscaledDeltaTime * 0.5f;
                screenTearMaterial.SetFloat("_TearAmount", tear);
                yield return null;
            }
        }
    }
}
