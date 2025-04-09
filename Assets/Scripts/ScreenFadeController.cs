using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScreenFadeController : MonoBehaviour
{
    [Header("Images classiques UI à fondre")]
    public List<Image> uiImages; // <== Image (non Raw)

    [Header("RawImages (vidéos ou autres) à fondre")]
    public List<RawImage> rawImages; // <== RawImage

    [Header("Durée du fondu")]
    public float fadeDuration = 2.5f;

    [Header("Démarrage automatique")]
    public bool autoStart = true;

    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private float delayBeforeDisable = 2f;

    private void Start()
    {
        if (autoStart)
            StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        float timer = 0f;

        // Prend la couleur initiale comme base
        Color baseColor = Color.black;
        if (uiImages.Count > 0 && uiImages[0] != null)
            baseColor = uiImages[0].color;
        else if (rawImages.Count > 0 && rawImages[0] != null)
            baseColor = rawImages[0].color;

        Color startColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        Color endColor = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        // Appliquer fade progressif
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            foreach (var img in uiImages)
                if (img != null)
                    img.color = Color.Lerp(startColor, endColor, t);

            foreach (var raw in rawImages)
                if (raw != null)
                    raw.color = Color.Lerp(startColor, endColor, t);

            yield return null;
        }

        // Fin du fade : couleur finale
        foreach (var img in uiImages)
            if (img != null)
                img.color = endColor;

        foreach (var raw in rawImages)
            if (raw != null)
                raw.color = endColor;

        OnFadeComplete();
    }

    private void OnFadeComplete()
    {
        Debug.Log("Fade terminé (UI + RawImage).");
        StartCoroutine(DisableFadeCanvasAfterDelay());
    }

    private IEnumerator DisableFadeCanvasAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeDisable);

        if (fadeCanvas != null)
            fadeCanvas.SetActive(false);
    }

    public void TriggerFadeOut()
    {
        StartCoroutine(FadeOut());
    }
}
