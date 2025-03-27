using UnityEngine;
using Fusion;

public class AtomSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject atomPrefab;

    public void SpawnAtom(Vector3 position, Vector3 initialVelocity)
    {
        if (HasStateAuthority)
        {
            Runner.Spawn(atomPrefab, position, Quaternion.identity, Runner.LocalPlayer, 
                (runner, obj) => {
                    var atom = obj.GetComponent<Atom>();
                    atom.Velocity = initialVelocity;
                });
        }
    }
}
