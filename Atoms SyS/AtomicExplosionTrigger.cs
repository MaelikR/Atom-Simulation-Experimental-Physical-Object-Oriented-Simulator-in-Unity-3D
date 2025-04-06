using UnityEngine;
using Fusion;

public class AtomicExplosionTrigger : NetworkBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 15f;
    public float energyAmount = 250f;
    public float explosionForce = 1500f;
    public float screenShakeIntensity = 1.5f;
    public float screenShakeDuration = 1.2f;

    [Header("Visual & Audio Feedback")]
    public GameObject explosionVFX;
    public AudioClip explosionSound;
    public Light flashLight;
    public float flashIntensity = 8f;
    public float flashDuration = 0.3f;

    [Header("UI Feedback")]
    public GameObject chargeUI;
    public Animator chargeAnimator;

    private bool isCharging = false;

    public void StartCharge()
    {
        if (chargeAnimator != null)
        {
            chargeAnimator.SetTrigger("StartCharge");
        }
        isCharging = true;
    }

    public void Detonate()
    {
        isCharging = false;

        if (chargeAnimator != null)
        {
            chargeAnimator.SetTrigger("Detonate");
        }

        // VFX + son
        if (explosionVFX) Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if (explosionSound) AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Flash lumineux
        if (flashLight != null)
        {
            StartCoroutine(FlashRoutine());
        }


        // Affecte tous les atomes autour
        Collider[] hitAtoms = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var col in hitAtoms)
        {
            Atom atom = col.GetComponent<Atom>();
            if (atom != null)
            {
                atom.AddEnergy(energyAmount); // surcharge
                Rigidbody rb = atom.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (atom.transform.position - transform.position).normalized;
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCharge();
        }

        if (Input.GetKeyDown(KeyCode.B) && isCharging)
        {
            Detonate();
        }
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        flashLight.intensity = flashIntensity;
        yield return new WaitForSeconds(flashDuration);
        flashLight.intensity = 0f;
    }
}
