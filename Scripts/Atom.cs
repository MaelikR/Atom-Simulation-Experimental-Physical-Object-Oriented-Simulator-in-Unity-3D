using UnityEngine;
using Fusion;

public class Atom : NetworkBehaviour
{
    [Networked] public Vector3 Velocity { get; set; }

    private Rigidbody _rb;

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
    }
using UnityEngine;
using Fusion;

public class Atom : NetworkBehaviour
{
    [Networked] public Vector3 Velocity { get; set; }

    private Rigidbody _rb;

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public float energy = 0f;

    public void AddEnergy(float amount)
    {
        energy += amount;
    }

    public void RemoveEnergy(float amount)
    {
        energy = Mathf.Max(0f, energy - amount);
    }
    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && _rb != null)
        {
            // Simulation de mouvement simple
            _rb.velocity = Velocity;
        }
        else if (_rb != null)
        {
            // Application côté client
            _rb.velocity = Velocity;
        }
    }
}

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && _rb != null)
        {
            // Simulation de mouvement simple
            _rb.velocity = Velocity;
        }
        else if (_rb != null)
        {
            // Application côté client
            _rb.velocity = Velocity;
        }
    }
}
