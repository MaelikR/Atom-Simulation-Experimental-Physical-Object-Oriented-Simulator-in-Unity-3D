// HealthSystemNetworked.cs
using UnityEngine;
using Fusion;

public class HealthSystemNetworked : NetworkBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [Networked] public float currentHealth { get; set; }

    public GameObject healthBarPrefab;
    private HealthBarUI healthBar;

    [Header("Optional")]
    public GameObject deathEffect;
    public AudioClip deathSound;
    public bool destroyOnDeath = true;

    private AudioSource audioSource;

    public override void Spawned()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

        if (healthBarPrefab)
        {
            var ui = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBar = ui.GetComponent<HealthBarUI>();
            if (healthBar != null)
                healthBar.Setup(transform, maxHealth);
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        currentHealth -= damage;

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (deathEffect != null)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            Destroy(effect, ps != null ? ps.main.duration + ps.main.startLifetime.constantMax : 3f);
        }

        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        if (destroyOnDeath)
        {
            Runner.Despawn(Object); // Fusion compliant
        }
        else
        {
            GetComponent<Collider>().enabled = false;
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.isKinematic = true;
        }
    }
}