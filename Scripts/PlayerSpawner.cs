using Fusion;
using UnityEngine;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [SerializeField] private Transform spawnPoint;

    public void PlayerJoined(PlayerRef player)
    {
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
            
            // ✅ Lien entre le PlayerRef et le NetworkObject
            Runner.SetPlayerObject(player, playerObject);
        }
        else
        {
            Debug.LogError($"[PlayerSpawner] Failed to spawn player {player.PlayerId}.");
        }
    }
}
