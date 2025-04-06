
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroCinematic : MonoBehaviour
{
    public Camera cinematicCamera;
    public Transform startPoint;
    public Transform endPoint;
    public float travelSpeed = 2f;

    public Text dialogueText;
    public CanvasGroup canvasGroup;
    public string[] poeticLines;
    public float textDelay = 6f;


    public GameObject introUI;
    public AudioSource backgroundMusic;
    public AudioClip pianoIntro;

  //  private VoiceOnControl voiceScript;
    private bool hasStarted = false;

    void Start()
    {
        StartCoroutine(PreloadAssets());

        cinematicCamera.transform.position = startPoint.position;
        cinematicCamera.transform.rotation = startPoint.rotation;

        introUI.SetActive(true);

     //   voiceScript = FindObjectOfType<VoiceOnControl>();

        canvasGroup.alpha = 0;
        StartCoroutine(PlayIntro());
    }

    IEnumerator PreloadAssets()
    {
        yield return new WaitForSeconds(0.5f);

       // GameObject preloadButterfly = Instantiate(Resources.Load<GameObject>("Prefabs/Butterfly"));
        //preloadButterfly.SetActive(false);
        yield return null;

       // GameObject preloadAtomFX = Instantiate(Resources.Load<GameObject>("Particles/AtomAura"));
        //preloadAtomFX.SetActive(false);
        yield return null;

        if (pianoIntro != null && backgroundMusic != null)
        {
            backgroundMusic.clip = pianoIntro;
            backgroundMusic.Play();
        }
    }

    IEnumerator PlayIntro()
    {
        hasStarted = true;

        StartCoroutine(ShowPoeticLines());

        float journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
        float startTime = Time.time;

        while (Vector3.Distance(cinematicCamera.transform.position, endPoint.position) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * travelSpeed;
            float fraction = distCovered / journeyLength;
            cinematicCamera.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fraction);
            cinematicCamera.transform.rotation = Quaternion.Slerp(startPoint.rotation, endPoint.rotation, fraction);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        EndIntro();
    }

    IEnumerator ShowPoeticLines()
    {
        foreach (string line in poeticLines)
        {
            yield return StartCoroutine(FadeInText(line));
            yield return new WaitForSeconds(textDelay);
            yield return StartCoroutine(FadeOutText());
        }
    }

    IEnumerator FadeInText(string line)
    {
        dialogueText.text = line;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 1.5f;
            canvasGroup.alpha = Mathf.SmoothStep(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeOutText()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 1.5f;
            canvasGroup.alpha = Mathf.SmoothStep(1, 0, t);
            yield return null;
        }
        dialogueText.text = "";
    }

    void EndIntro()
    {
        cinematicCamera.gameObject.SetActive(false);

        //voiceScript?.OnPlayerControlActivated();
        introUI.SetActive(false);

        Resources.UnloadUnusedAssets();
    }
}
