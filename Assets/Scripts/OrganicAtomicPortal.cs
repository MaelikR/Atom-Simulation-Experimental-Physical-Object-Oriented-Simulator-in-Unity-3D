using UnityEngine;

public class OrganicAtomicPortal : MonoBehaviour
{
    [Header("Teleportation Settings")]
    public Transform destinationPoint;
    public float activationDelay = 1f;
    public bool requireStabilizedAtoms = true;

    [Header("Visual & Audio Effects")]
    public ParticleSystem dissolveEffect;
    public ParticleSystem regrowEffect;
    public AudioClip organicTeleportSound;

    [Header("Organic Check")]
    public AtomStabilityChecker stabilityChecker;

    private bool isPlayerInside = false;
    private GameObject playerRef;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (requireStabilizedAtoms && stabilityChecker != null)
            {
                if (!stabilityChecker.isStable) return;

            }

            playerRef = other.gameObject;
            StartCoroutine(TeleportSequence());
        }
    }

    System.Collections.IEnumerator TeleportSequence()
    {
        if (dissolveEffect) dissolveEffect.Play();

        if (organicTeleportSound && playerRef.TryGetComponent(out AudioSource audio))
        {
            audio.PlayOneShot(organicTeleportSound);
        }

        yield return new WaitForSeconds(activationDelay);

        if (destinationPoint != null)
        {
            playerRef.transform.position = destinationPoint.position;
        }

        if (regrowEffect) regrowEffect.Play();
    }
}
