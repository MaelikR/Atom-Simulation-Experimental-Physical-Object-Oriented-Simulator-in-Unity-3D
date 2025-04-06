using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private Transform safeZone;

    public void PlayerJoined(PlayerRef player)
    {
        if (!playerPrefab.IsValid || safeZone == null || Runner == null)
        {
            Debug.LogWarning("[PlayerSpawner] Invalid setup.");
            return;
        }

        if (Runner.TryGetPlayerObject(player, out _))
        {
            Debug.Log($"[PlayerSpawner] Player {player.PlayerId} already spawned. Skipping.");
            return;
        }

        Vector3 spawnPosition = safeZone.position;
        spawnPosition.y = 2.5f; // ou même 2f si besoin


        NetworkObject playerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        if (playerObject != null)
            Debug.Log($"[PlayerSpawner] Spawned player {player.PlayerId} at {spawnPosition}");
        else
            Debug.LogError($"[PlayerSpawner] Failed to spawn player {player.PlayerId}.");
    }
}
