// AtomSpawner.cs — spawn d'atomes avec chance d'être une créature vivante
using UnityEngine;
using Fusion;

public class AtomSpawnerCreature : NetworkBehaviour
{
    [SerializeField] private GameObject atomPrefab;
    [SerializeField] private int atomCount = 50;
    [SerializeField] private float spreadRadius = 5f;
    [SerializeField] private float creatureChance = 0.2f;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            for (int i = 0; i < atomCount; i++)
            {
                Vector3 spawnPos = transform.position + Random.insideUnitSphere * spreadRadius;
                spawnPos.y = transform.position.y; // stay flat

                Runner.Spawn(atomPrefab, spawnPos, Quaternion.identity, Runner.LocalPlayer, (runner, obj) =>
                {
                    var atom = obj.GetComponent<Atom>();

                    if (Random.value < creatureChance)
                    {
                        obj.AddComponent<AtomCreature>();
                    }

                    atom.Velocity = Random.insideUnitSphere * 0.5f;
                });
            }
        }
    }
}
