using UnityEngine;
using Fusion;

public class AtomSpawner : NetworkBehaviour
{
    public GameObject atomPrefabGO;
    private NetworkPrefabRef atomPrefabRef;

    public override void Spawned()
    {
        // Crée une référence NetworkPrefabRef à partir du GameObject enregistré
        atomPrefabRef = Runner.GetPrefabRef(atomPrefabGO);
    }

    public void SpawnAtom(Vector3 position, Vector3 initialVelocity)
    {
        if (HasStateAuthority)
        {
            Runner.Spawn(atomPrefabRef, position, Quaternion.identity, Runner.LocalPlayer, 
                (runner, obj) => {
                    var atom = obj.GetComponent<Atom>();
                    atom.Velocity = initialVelocity;
                });
        }
    }
}
