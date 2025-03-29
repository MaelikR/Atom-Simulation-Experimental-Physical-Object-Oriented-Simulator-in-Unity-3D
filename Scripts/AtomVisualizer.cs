// =========================
// AtomVisualizer.cs — Networked + Evolving Organism Version
// =========================
using UnityEngine;
using Fusion;

public class AtomVisualizer : NetworkBehaviour
{
    [Header("Atom Settings")]
    public NetworkPrefabRef atomPrefab;
    public int numberOfVisualAtoms = 1000;
    public float spreadRadius = 0.3f;
    public float mutationInterval = 5f;
    public float mutationStrength = 0.1f;

    private float mutationTimer;
    private NetworkObject[] spawnedAtoms;

    public override void Spawned()
    {
        if (!HasStateAuthority) return;

        spawnedAtoms = new NetworkObject[numberOfVisualAtoms];

        for (int i = 0; i < numberOfVisualAtoms; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spreadRadius;
            Runner.Spawn(atomPrefab, randomPos, Quaternion.identity, Object.InputAuthority, (runner, obj) =>
            {
                obj.transform.SetParent(transform);
                spawnedAtoms[i] = obj;
            });
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || spawnedAtoms == null) return;

        mutationTimer += Runner.DeltaTime;
        if (mutationTimer >= mutationInterval)
        {
            MutateAtoms();
            mutationTimer = 0f;
        }
    }

    void MutateAtoms()
    {
        foreach (var atom in spawnedAtoms)
        {
            if (atom == null) continue;

            Vector3 offset = Random.insideUnitSphere * mutationStrength;
            atom.transform.position += offset;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spreadRadius);
    }
}
