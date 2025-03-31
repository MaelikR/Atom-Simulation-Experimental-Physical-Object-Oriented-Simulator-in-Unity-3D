using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Optional")]
    public GameObject deathEffect;
    public AudioClip deathSound;
    public bool destroyOnDeath = true;

    private AudioSource audioSource;

    void Awake()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage from {attacker.name}. Remaining health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Tu peux remplacer ceci par une animation de mort ou flottement si nécessaire
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            // Ex : désactiver composants
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
