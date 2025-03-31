using UnityEngine;
using Fusion;
using System.Collections.Generic;

public class AtomStructure : NetworkBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject protonPrefab;
    [SerializeField] private GameObject neutronPrefab;
    [SerializeField] private GameObject electronPrefab;
    [SerializeField] private LineRenderer orbitRendererPrefab;

    [Networked] public int ProtonCount { get; set; } = 1;
    [Networked] public int NeutronCount { get; set; } = 0;
    [Networked] public int ElectronCount { get; set; } = 1;

    [Header("Structure")]
    [SerializeField] private Transform nucleus;
    [SerializeField] private Transform electronShell;
    [SerializeField] private float orbitRadius = 1.5f;
    [SerializeField] private float orbitSpeed = 100f;

    [Header("Electron Shells")]
    [SerializeField] private int[] shellCapacity = { 2, 8, 18, 32 }; // K, L, M, N

    private GameObject[] electrons;
    private List<LineRenderer> orbitRenderers = new();

    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;

        SpawnNucleus();
        SpawnElectrons();
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
        orbitRenderers.ForEach(r => Destroy(r.gameObject));
        orbitRenderers.Clear();

        int level = 0, countInLevel = 0;
        int capacity = shellCapacity[level];

        for (int i = 0; i < ElectronCount; i++)
        {
            if (countInLevel >= capacity)
            {
                level++;
                countInLevel = 0;
                capacity = shellCapacity[Mathf.Clamp(level, 0, shellCapacity.Length - 1)];
            }

            float angle = countInLevel * Mathf.PI * 2 / capacity;
            float radius = orbitRadius * (level + 1);
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            int ei = i, lvl = level;
            Runner.Spawn(electronPrefab, electronShell.position + pos, Quaternion.identity, Object.InputAuthority, (runner, obj) =>
            {
                obj.transform.SetParent(electronShell);
                obj.name = $"Electron_{ei}_Shell_{lvl}";
                electrons[ei] = obj;
            });

            // Orbit visual
            if (countInLevel == 0)
            {
                var orbitVisual = Instantiate(orbitRendererPrefab, electronShell);
                orbitVisual.positionCount = 100;
                for (int p = 0; p < 100; p++)
                {
                    float a = p / 100f * Mathf.PI * 2;
                    orbitVisual.SetPosition(p, new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a)) * radius);
                }
                orbitRenderers.Add(orbitVisual);
            }

            countInLevel++;
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

    [ContextMenu("Rebuild Atom")]
    public void RebuildAtom()
    {
        if (!Object.HasStateAuthority) return;

        foreach (Transform t in nucleus) Destroy(t.gameObject);
        foreach (Transform t in electronShell) Destroy(t.gameObject);

        SpawnNucleus();
        SpawnElectrons();
    }
}
