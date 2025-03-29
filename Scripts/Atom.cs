// =========================
// Atom.cs — Improved Version
// =========================
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
public class Atom : NetworkBehaviour
{
    [Networked] public Vector3 Velocity { get; set; }

    [Header("Atom State")]
    public float energy = 0f;

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
        // Optional: visual feedback or state change
    }

    public void RemoveEnergy(float amount)
    {
        energy = Mathf.Max(0f, energy - amount);
        // Optional: visual feedback or decay effect
    }

    public override void FixedUpdateNetwork()
    {
        if (_rb == null) return;

        // Apply consistent networked velocity
        _rb.velocity = Velocity;
    }
}
