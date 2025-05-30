using UnityEngine;
using System.Collections;
public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject healthBarPrefab;

    private HealthBarUI healthBar;
    [Header("Optional")]
    public GameObject deathEffect;
    public AudioClip deathSound;
    public bool destroyOnDeath = true;

    private AudioSource audioSource;

    void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        if (healthBarPrefab)
        {
            var ui = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBar = ui.GetComponent<HealthBarUI>();
            healthBar.Setup(transform, maxHealth);
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        currentHealth -= damage;
      
        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);

            // Détruire automatiquement après la durée du ParticleSystem
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(effect, 3f); // fallback
            }
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;
        }
    }

}
