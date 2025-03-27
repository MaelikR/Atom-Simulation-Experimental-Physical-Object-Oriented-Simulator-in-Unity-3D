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
