using UnityEngine;
using Fusion;
using System.Collections;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [Header("Health Settings")]
    [SerializeField] private float baseMaxHealth = 100f;

    [Networked] public float MaxHealth { get; private set; }
    [Networked] public float CurrentHealth { get; private set; }

    public float damageCooldown = 1.5f;
    public float respawnDelay = 3f;
    public Transform respawnPoint;
    public GameObject healthBarPrefab;
    [Networked] private float lastDamageTime { get; set; }

    private HealthBarUI healthBar;

    public override void Spawned()
    {
        CurrentHealth = MaxHealth;
        lastDamageTime = -999f;

        if (Object.HasInputAuthority)
            SpawnHealthBar();
    }

    public void TakeDamage(float amount, GameObject source = null)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        CurrentHealth -= amount;
        lastDamageTime = Time.time;

        if (CurrentHealth <= 0)
            Die();

    }

    public void Heal(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
       
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(float amount)
    {
        if (CurrentHealth <= 0) return;
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0);

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        if (Object.HasInputAuthority)
            Debug.Log("☠ Player has died!");

        Runner.StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        CurrentHealth = MaxHealth;

        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            SpawnHealthBar();
    }

    void SpawnHealthBar()
    {
        if (healthBarPrefab == null) return;

        var bar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        healthBar = bar.GetComponent<HealthBarUI>();
        //if (healthBar != null)
           // healthBar.Setup(this); // ✅ Corrigé : passe l'instance PlayerHealth
    }
}
