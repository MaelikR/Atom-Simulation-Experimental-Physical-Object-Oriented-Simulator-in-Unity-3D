// IntroCinematicController.cs
using UnityEngine;
using TMPro;
using System.Collections;

public class IntroCinematic : MonoBehaviour
{
    public Transform cameraRig;
    public Transform cameraTarget;
    public float travelSpeed = 2f;

    public TextMeshProUGUI introText;
    public CanvasGroup fadeCanvas;
    public float textDelay = 6f;

    [TextArea(3, 10)]
    public string[] poeticLines = new string[]
    {
        "In the beginning, there was silence.",
        "No sound. No light. Only the weight of possibility.",
        "From the void, fragments danced — nameless, weightless.",
        "Atoms — the first whispers of existence.",
        "They collided, merged, and drifted apart, rewriting chaos.",
        "Stars were born in the furnace of that ancient dust.",
        "And from their ashes… came water.",
        "In water, the great mystery unfolded.",
        "Molecules learned to breathe. To bond. To dream.",
        "And somewhere, in a silent ocean…",
        "Life blinked awake.",
        "Not with a roar — but with a ripple.",
        "You are made of that same ancient dust.",
        "Of atoms that burned in stars, wept in rain, lived in beasts.",
        "You are… the echo of everything.",
        "And now…",
        "The ocean remembers you."
    };

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro()
    {
        fadeCanvas.alpha = 1;
        introText.text = "";

        // Fade in
        yield return StartCoroutine(FadeCanvas(0, 2f));

        // Start moving camera while displaying lines
        StartCoroutine(MoveCamera());
        yield return StartCoroutine(DisplayPoeticLines());
    }

    IEnumerator MoveCamera()
    {
        while (Vector3.Distance(cameraRig.position, cameraTarget.position) > 0.05f)
        {
            cameraRig.position = Vector3.Lerp(cameraRig.position, cameraTarget.position, Time.deltaTime * travelSpeed);
            yield return null;
        }
    }

    IEnumerator DisplayPoeticLines()
    {
        foreach (string line in poeticLines)
        {
            introText.text = line;
            yield return new WaitForSeconds(textDelay);
        }
    }

    IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        float startAlpha = fadeCanvas.alpha;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            yield return null;
        }
    }
}
