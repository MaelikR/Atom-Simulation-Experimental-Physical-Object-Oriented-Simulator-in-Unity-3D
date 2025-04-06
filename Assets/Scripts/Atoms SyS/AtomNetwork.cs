using UnityEngine;
using Fusion;

public class AtomNetwork : NetworkBehaviour
{
    [Networked] public Vector3 Velocity { get; set; }

    private Rigidbody _rigidbody;

    public override void Spawned()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            // Exemple de simple mouvement pseudo-aléatoire
            Velocity += Random.insideUnitSphere * 0.1f;
            _rigidbody.velocity = Velocity;
        }
        else
        {
            // Application côté client (prédiction ou interpolation)
            _rigidbody.velocity = Velocity;
        }
    }
}
