// NonEuclideanPortal.cs
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class NonEuclideanPortal : NetworkBehaviour
{
    [Header("Portail de sortie")]
    public Transform linkedPortal;
    public Vector3 positionOffset = Vector3.zero;
    public bool rotateOnTeleport = true;

    [Header("Effets Visuels & Audio")]
    public ParticleSystem teleportEffect;
    public AudioClip portalSound;

    [Header("Cooldown & Téléportation")]
    public float cooldown = 0.5f;
    private float lastTeleportTime;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (linkedPortal == null || !other.CompareTag("Player")) return;
        if (Time.time - lastTeleportTime < cooldown) return;
        lastTeleportTime = Time.time;

        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            other.transform.position = linkedPortal.position + positionOffset;

            if (rotateOnTeleport)
                other.transform.rotation = linkedPortal.rotation;

            if (teleportEffect != null) teleportEffect.Play();
            if (audioSource != null && portalSound != null)
                audioSource.PlayOneShot(portalSound);

            controller.enabled = true;
        }
    }
}