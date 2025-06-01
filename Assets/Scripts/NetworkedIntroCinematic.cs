namespace Fusion
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;
    using Fusion;

    public class NetworkedIntroCinematic : MonoBehaviour
    {
        [Header("Cinematic")]
        public Camera cinematicCamera;
        public Transform startPoint;
        public Transform endPoint;
        public float travelSpeed = 2f;

        [Header("Dialogue")]
        public Text dialogueText;
        public CanvasGroup canvasGroup;
        public string[] poeticLines;
        public float textDelay = 6f;

        [Header("Scene References")]
        public GameObject playerController;
        public GameObject introUI;
        public AudioSource backgroundMusic;
        public AudioClip pianoIntro;

        [Header("Config")]
        public float startDelay = 1.5f;

        private VoiceOnControl voiceScript;
        private bool hasStarted = false;

        public void Start()
        {
            cinematicCamera.gameObject.SetActive(false);
            introUI.SetActive(false);
            return;

            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(startDelay);


            playerController.SetActive(false);
            introUI.SetActive(true);
            canvasGroup.alpha = 0;

            cinematicCamera.transform.position = startPoint.position;
            cinematicCamera.transform.rotation = startPoint.rotation;

            voiceScript = FindObjectOfType<VoiceOnControl>();

            StartCoroutine(PreloadAssets());
            StartCoroutine(PlayIntro());
        }

        IEnumerator PreloadAssets()
        {
            yield return new WaitForSeconds(0.5f);

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
        public void TriggerAsHost()
        {
          

            StartCoroutine(DelayedStart()); // tu peux renommer si besoin
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
            playerController.SetActive(true);
            voiceScript?.OnPlayerControlActivated();
            introUI.SetActive(false);
            Resources.UnloadUnusedAssets();
        }
    }
}
