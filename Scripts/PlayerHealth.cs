using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public HealthBarUI healthBarPrefab;
    private HealthBarUI healthBar;

    [Header("Respawn")]
    public Transform respawnPoint;
    public float respawnDelay = 3f;

    public bool IsDead => currentHealth <= 0f;

    void Start()
    {
        currentHealth = maxHealth;
        SpawnHealthBar();
    }

    void SpawnHealthBar()
    {
        if (healthBarPrefab != null)
        {
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBar.Setup(transform, maxHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar prefab not assigned.");
        }
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (healthBar != null)
            healthBar.UpdateValue(currentHealth);

        if (currentHealth <= 0f)
            StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        currentHealth = maxHealth;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        if (healthBar != null)
            healthBar.UpdateValue(currentHealth);
    }
}
