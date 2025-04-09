using Fusion;
using UnityEngine;

public class PlayerSpawnerNetworked : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] private NetworkPrefabRef playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (!playerPrefab.IsValid || Runner == null)
        {
            Debug.LogWarning("[PlayerSpawner] Invalid setup.");
            return;
        }

        if (Runner.TryGetPlayerObject(player, out _))
        {
            Debug.Log($"[PlayerSpawner] Player {player.PlayerId} already spawned. Skipping.");
            return;
        }

        // Cherche un Transform avec un nom correspondant à l'entrée
        string entryName = PortalTransferData.EntryPortalName;
        GameObject foundSpawn = GameObject.Find($"SpawnPoint_{entryName}");

        Vector3 spawnPosition = Vector3.zero;

        if (foundSpawn != null)
        {
            spawnPosition = foundSpawn.transform.position;
            spawnPosition.y = 2.5f;
            Debug.Log($"[PlayerSpawner] Spawning at linked entry point: {entryName}");
        }
        else
        {
            Debug.LogWarning($"[PlayerSpawner] Entry point {entryName} not found, using fallback.");
            spawnPosition = new Vector3(Random.Range(-3f, 3f), 2.5f, Random.Range(-3f, 3f));
        }

        NetworkObject playerObject = Runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

        if (playerObject != null)
            Debug.Log($"[PlayerSpawner] Spawned player {player.PlayerId} at {spawnPosition}");
        else
            Debug.LogError($"[PlayerSpawner] Failed to spawn player {player.PlayerId}.");
    }
}
