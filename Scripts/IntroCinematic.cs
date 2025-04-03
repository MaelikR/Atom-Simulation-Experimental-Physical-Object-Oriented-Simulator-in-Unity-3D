using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class IntroCinematic : MonoBehaviour
{
    public Camera cinematicCamera;
    public Transform cameraPathStart;
    public Transform cameraPathEnd;
    public float travelDuration = 10f;
    public GameObject playerController;
    public CanvasGroup fadeCanvas;
    public TextMeshProUGUI cinematicText;

    [TextArea(4, 10)]
    public string[] poeticLines;
    public float textDisplayTime = 4f;

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        playerController.SetActive(false);
        cinematicCamera.gameObject.SetActive(true);

        yield return StartCoroutine(FadeFromBlack());
        yield return StartCoroutine(TravelCamera());
        yield return StartCoroutine(DisplayPoeticText());

        cinematicCamera.gameObject.SetActive(false);
        playerController.SetActive(true);
    }

    IEnumerator FadeFromBlack()
    {
        fadeCanvas.alpha = 1;
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.deltaTime;
            yield return null;
        }
        fadeCanvas.blocksRaycasts = false;
    }

    IEnumerator TravelCamera()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / travelDuration;
            cinematicCamera.transform.position = Vector3.Lerp(cameraPathStart.position, cameraPathEnd.position, t);
            cinematicCamera.transform.LookAt(Vector3.Lerp(cameraPathStart.forward, cameraPathEnd.forward, t));
            yield return null;
        }
    }

    IEnumerator DisplayPoeticText()
    {
        cinematicText.gameObject.SetActive(true);

        foreach (string line in poeticLines)
        {
            cinematicText.text = line;
            yield return new WaitForSeconds(textDisplayTime);
        }

        cinematicText.text = "";
        cinematicText.gameObject.SetActive(false);
    }
}
