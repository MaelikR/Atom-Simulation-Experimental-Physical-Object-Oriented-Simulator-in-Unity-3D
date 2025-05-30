// NetworkPortal.cs
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public class NetworkPortal : NetworkBehaviour
{
    [Header("Portal Settings")]
    public string destinationRoomName;
    public Transform portalEffectPosition;
    public GameObject teleportVFX;
    public float cooldown = 2f;

    private bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (!Object.HasInputAuthority || isTeleporting) return;
        if (!other.CompareTag("Player")) return;

        isTeleporting = true;
        if (teleportVFX != null)
            Instantiate(teleportVFX, portalEffectPosition.position, Quaternion.identity);

        Invoke(nameof(LeaveAndJoin), 1.2f); // Petit délai stylé avant téléportation
    }

    void LeaveAndJoin()
    {
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();
        if (runner != null)
        {
            runner.Shutdown(true, ShutdownReason.Ok);
            runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = destinationRoomName,
                Scene = SceneRef.FromIndex(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex),
                SceneManager = runner.GetComponent<NetworkSceneManagerDefault>()
            });
        }
        else
        {
            Debug.LogError("No NetworkRunner found in scene");
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (isTeleporting && cooldown > 0f)
        {
            cooldown -= Runner.DeltaTime;
        }
    }
}