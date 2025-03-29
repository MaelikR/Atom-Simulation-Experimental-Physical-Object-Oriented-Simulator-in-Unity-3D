// =========================
// PlayerSpawner.cs — Simplified for Direct Player Spawn Only
// =========================
using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private Transform spawnPoint;

    public void PlayerJoined(PlayerRef player)
    {
        if (player != Runner.LocalPlayer) return;

        if (!playerPrefab.IsValid)
        {
            Debug.LogError("[PlayerSpawner] Player prefab not assigned or invalid.");
            return;
        }

        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        spawnPosition.y = 0f;

        NetworkObject playerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        if (playerObject != null)
        {
            Debug.Log($"[PlayerSpawner] Player {player.PlayerId} spawned at {spawnPosition}.");
        }
        else
        {
            Debug.LogError($"[PlayerSpawner] Failed to spawn player {player.PlayerId}.");
        }
    }
}
