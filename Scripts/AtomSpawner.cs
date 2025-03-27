using UnityEngine;
using Fusion;

public class AtomSpawner : NetworkBehaviour
{
    public NetworkPrefabRef atomPrefab;

    public void SpawnAtom(Vector3 position, Vector3 initialVelocity)
    {
        if (HasStateAuthority)
        {
            Runner.Spawn(atomPrefab, position, Quaternion.identity, Runner.LocalPlayer, 
                (runner, obj) => {
                    var atom = obj.GetComponent<AtomNetwork>();
                    atom.Velocity = initialVelocity;
                });
        }
    }
}
