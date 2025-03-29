// =========================
// AtomStructure.cs — Improved Fusion-ready Version
// =========================
using UnityEngine;
using Fusion;

public class AtomStructure : NetworkBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject protonPrefab;
    [SerializeField] private GameObject neutronPrefab;
    [SerializeField] private GameObject electronPrefab;

    [Networked] public int ProtonCount { get; set; } = 1;
    [Networked] public int NeutronCount { get; set; } = 0;
    [Networked] public int ElectronCount { get; set; } = 1;

    [Header("Structure")]
    [SerializeField] private Transform nucleus;
    [SerializeField] private Transform electronShell;
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 100f;

    private GameObject[] electrons;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            SpawnNucleus();
            SpawnElectrons();
        }
    }

    private void SpawnNucleus()
    {
        for (int i = 0; i < ProtonCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 0.2f;
            Runner.Spawn(protonPrefab, nucleus.position + offset, Quaternion.identity, Object.InputAuthority, (runner, obj) =>
            {
                obj.transform.SetParent(nucleus);
                obj.name = $"Proton_{i}";
            });
        }

        for (int i = 0; i < NeutronCount; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 0.2f;
            Runner.Spawn(neutronPrefab, nucleus.position + offset, Quaternion.identity, Object.InputAuthority, (runner, obj) =>
            {
                obj.transform.SetParent(nucleus);
                obj.name = $"Neutron_{i}";
            });
        }
    }

    private void SpawnElectrons()
    {
        electrons = new GameObject[ElectronCount];

        for (int i = 0; i < ElectronCount; i++)
        {
            float angle = i * Mathf.PI * 2 / ElectronCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;

            Runner.Spawn(electronPrefab, electronShell.position + pos, Quaternion.identity, Object.InputAuthority, (runner, obj) =>
            {
                obj.transform.SetParent(electronShell);
                obj.name = $"Electron_{i}";
                electrons[i] = obj.gameObject;
            });
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority || electrons == null) return;

        for (int i = 0; i < electrons.Length; i++)
        {
            if (electrons[i] != null)
            {
                electrons[i].transform.RotateAround(electronShell.position, Vector3.up, orbitSpeed * Runner.DeltaTime);
            }
        }
    }
}
