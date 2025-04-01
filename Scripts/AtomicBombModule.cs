using UnityEngine;
using Fusion;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]
public class AtomicBombModule : NetworkBehaviour
{
    [Header("Activation Settings")]
    public float overloadThreshold = 50f;
    public float radius = 6f;
    public float magneticForce = 20f;
    public float energyGainPerAtom = 1f;
    public float cooldownAfterExplosion = 10f;

    [Header("Visual & Audio")] 
    public GameObject explosionEffectPrefab;
    public AudioClip explosionSound;
    public AudioClip chargeSound;
    public Light chargeLight;
    public AnimationCurve lightIntensityOverCharge;

    private float currentEnergy = 0f;
    private bool hasExploded = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (chargeLight) chargeLight.intensity = 0f;
    }

    void FixedUpdate()
    {
        if (hasExploded) return;

        var atoms = GameObject.FindGameObjectsWithTag("Atom");
        foreach (var atom in atoms)
        {
            float dist = Vector3.Distance(transform.position, atom.transform.position);
            if (dist < radius)
            {
                var rb = atom.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (transform.position - atom.transform.position).normalized;
                    rb.AddForce(dir * magneticForce);
                }
                currentEnergy += energyGainPerAtom * Time.fixedDeltaTime;
            }
        }

        UpdateChargingVisual();

        if (currentEnergy >= overloadThreshold)
        {
            RPC_TriggerExplosion();
        }
    }

    void UpdateChargingVisual()
    {
        if (chargeLight != null)
        {
            float t = Mathf.Clamp01(currentEnergy / overloadThreshold);
            chargeLight.intensity = lightIntensityOverCharge.Evaluate(t) * 5f;
        }

        if (audioSource != null && !audioSource.isPlaying && chargeSound != null)
        {
            audioSource.clip = chargeSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_TriggerExplosion()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (audioSource && explosionSound)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(explosionSound);
        }

        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 10f);
        }

        // Désintégrer les atomes dans le rayon
        var atoms = GameObject.FindGameObjectsWithTag("Atom");
        foreach (var atom in atoms)
        {
            if (Vector3.Distance(transform.position, atom.transform.position) < radius)
            {
                Destroy(atom);
            }
        }

        // Reset après cooldown (si boucle souhaitée)
        StartCoroutine(ResetAfterCooldown());
    }

    IEnumerator ResetAfterCooldown()
    {
        yield return new WaitForSeconds(cooldownAfterExplosion);
        hasExploded = false;
        currentEnergy = 0f;
        if (chargeLight) chargeLight.intensity = 0f;
    }
}
