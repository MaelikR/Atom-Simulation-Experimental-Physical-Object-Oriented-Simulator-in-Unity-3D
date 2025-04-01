// AtomicExplosionTrigger.cs
using UnityEngine;
using Fusion;

public class AtomicExplosionTrigger : NetworkBehaviour
{
    [Header("Explosion Settings")]
    public float energyThreshold = 100f;
    public float explosionRadius = 10f;
    public float explosionForce = 500f;
    public GameObject explosionEffect;
    public AudioClip explosionSound;

    [Header("Charge Settings")]
    public float currentEnergy = 0f;
    public float chargeRate = 10f; // Energy per second
    public bool autoTrigger = true;

    [Header("UI")] 
    public GameObject uiChargeBarPrefab;
    private UIAtomicChargeBar uiBar;

    private bool hasExploded = false;

    void Start()
    {
        if (uiChargeBarPrefab)
        {
            var canvas = Instantiate(uiChargeBarPrefab);
            uiBar = canvas.GetComponent<UIAtomicChargeBar>();
            uiBar.Setup(transform, energyThreshold);
        }
    }

    void Update()
    {
        if (hasExploded) return;

        currentEnergy += chargeRate * Time.deltaTime;

        if (uiBar)
            uiBar.UpdateValue(currentEnergy);

        if (autoTrigger && currentEnergy >= energyThreshold)
        {
            TriggerExplosion();
        }
    }

    [ContextMenu("Trigger Explosion")]
    public void TriggerExplosion()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        if (explosionSound)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            if (hit.TryGetComponent(out IDamageable dmg))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                float falloff = Mathf.Clamp01(1f - (dist / explosionRadius));
                dmg.TakeDamage(100f * falloff, gameObject);
            }
        }

        if (uiBar)
            Destroy(uiBar.gameObject);

        Destroy(gameObject, 0.2f);
    }
}
