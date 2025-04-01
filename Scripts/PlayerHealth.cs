using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    private float currentHealth;
    public GameObject healthBarPrefab;

    private HealthBarUI healthBar;
    private Transform healthBarInstance;
    public bool IsDead => currentHealth <= 0f;

    public Transform respawnPoint;
    public float respawnDelay = 3f;

    void Start()
    {
        currentHealth = maxHealth;

        SpawnHealthBar();
    }
    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        StartCoroutine(Respawn());
    }


    void SpawnHealthBar()
    {
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
            healthBar.UpdateValue(currentHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        currentHealth = maxHealth;

        // repositionnement
        if (respawnPoint != null)
            transform.position = respawnPoint.position;

        // réinitialiser la barre de vie
        if (healthBar != null)
            healthBar.UpdateValue(currentHealth);
        else
            SpawnHealthBar(); // <- important si elle a été détruite ou non instanciée
    }
}
