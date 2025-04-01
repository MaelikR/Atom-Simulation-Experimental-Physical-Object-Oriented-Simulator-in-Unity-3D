using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isDead = false;

    [Header("Respawn Settings")]
    public float respawnDelay = 5f;
    public Transform respawnPoint;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>(); // facultatif, selon ton controller
    }

    public void TakeDamage(float damage, GameObject attacker)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage from {attacker.name}. Remaining HP: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0f;

        Debug.Log($"{gameObject.name} has died.");

        if (animator != null)
            animator.SetTrigger("Die"); // Animation de mort si présente

        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        Respawn();
    }

    void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;

        if (respawnPoint != null)
        {
            if (controller != null) controller.enabled = false; // nécessaire si CharacterController
            transform.position = respawnPoint.position;
            if (controller != null) controller.enabled = true;
        }

        if (animator != null)
            animator.SetTrigger("Respawn"); // Animation de retour si définie

        Debug.Log($"{gameObject.name} has respawned.");
    }

    public float GetHealthPercent() => currentHealth / maxHealth;
}
