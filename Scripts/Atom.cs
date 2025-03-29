// =========================
// Atom.cs — Energy Surge & Instability Version
// =========================
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
public class Atom : NetworkBehaviour
{
    [Networked] public Vector3 Velocity { get; set; }

    [Header("Atom State")]
    public float energy = 0f;
    public float maxEnergy = 100f;
    public bool isUnstable = false;

    [Header("Explosion Effect")]
    public GameObject explosionEffectPrefab;
    public float explosionForce = 10f;
    public float explosionRadius = 3f;

    private Rigidbody _rb;

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb == null)
        {
            Debug.LogError("No Rigidbody found on Atom");
        }
    }

    public void AddEnergy(float amount)
    {
        energy += amount;
        if (energy >= maxEnergy && !isUnstable)
        {
            BecomeUnstable();
        }
    }

    public void RemoveEnergy(float amount)
    {
        energy = Mathf.Max(0f, energy - amount);
    }

    void BecomeUnstable()
    {
        isUnstable = true;
        TriggerExplosion();
    }

    void TriggerExplosion()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Collider[] nearby = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider col in nearby)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Runner.Despawn(Object); // Remove the atom from the network
    }

    public override void FixedUpdateNetwork()
    {
        if (_rb == null) return;
        _rb.velocity = Velocity;
    }
}
