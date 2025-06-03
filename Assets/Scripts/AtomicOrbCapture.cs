using UnityEngine;
using System.Collections;

public class AtomicOrbCapture : MonoBehaviour
{
    public float captureDuration = 3f;
    public KeyCode captureKey = KeyCode.F;
    public ParticleSystem captureEffect;
    public AudioSource captureSound;
    public GameObject orbVisual;
    public GameObject orbColliderZone;
    public string requiredItem = "AtomicContainer";

    private bool playerInRange = false;
    private bool isCapturing = false;
    private float captureTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            captureTimer = 0f;
        }
    }

    void Update()
    {
        if (!playerInRange || isCapturing) return;

        if (Input.GetKey(captureKey) && PlayerHasContainer())
        {
            captureTimer += Time.deltaTime;

            if (captureTimer >= captureDuration)
            {
                StartCoroutine(CaptureOrb());
            }
        }
        else
        {
            captureTimer = 0f;
        }
    }

    bool PlayerHasContainer()
    {
        // À personnaliser selon ton inventaire
        return InventorySystem.Instance.HasItem(requiredItem);
    }

    IEnumerator CaptureOrb()
    {
        isCapturing = true;

        // Effets visuels / sonores
        if (captureEffect) captureEffect.Play();
        if (captureSound) captureSound.Play();

        // Animation lévitation vers joueur (optionnel)
        float duration = 1.5f;
        Vector3 startPos = transform.position;
        Vector3 target = GameObject.FindWithTag("Player").transform.position + Vector3.up * 1.2f;

        float t = 0f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startPos, target, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        // Désactiver visuel et collider
        if (orbVisual) orbVisual.SetActive(false);
        if (orbColliderZone) orbColliderZone.SetActive(false);

        // Marquer la capture dans le système
        GameEvents.OnOrbCaptured?.Invoke();

        Destroy(gameObject); // ou déactiver
    }
}
