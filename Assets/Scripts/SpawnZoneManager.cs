using System.Collections.Generic;
using UnityEngine;

public class SpawnZoneManager : MonoBehaviour
{
    public static SpawnZoneManager Instance { get; private set; }

    private Dictionary<string, Transform> spawnPoints = new Dictionary<string, Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Remplit automatiquement les spawn points selon leur nom
        foreach (Transform child in transform)
        {
            if (!spawnPoints.ContainsKey(child.name))
            {
                spawnPoints.Add(child.name, child);
            }
        }
    }

    public Transform GetSpawnPoint(string portalName)
    {
        string formattedName = $"SpawnPoint_{portalName}";
        spawnPoints.TryGetValue(formattedName, out var point);
        return point;
    }
}
