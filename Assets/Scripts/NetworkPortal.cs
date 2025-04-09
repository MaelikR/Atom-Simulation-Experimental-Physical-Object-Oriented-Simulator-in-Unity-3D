using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections;
using UnityEngine.SceneManagement; // ← celle-ci !

public class NetworkPortal : NetworkBehaviour
{
    [Header("Portal Settings")]
    public string destinationRoomName;
    public Transform portalEffectPosition;
    public GameObject teleportVFX;
    public float cooldown = 2f;

    private bool isTeleporting = false;
    private NetworkRunner runner;

    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        if (runner == null)
        {
            Debug.LogError("No NetworkRunner found in scene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;
        if (!other.CompareTag("Player") || isTeleporting) return;
        if (teleportVFX != null)
            Instantiate(teleportVFX, portalEffectPosition.position, Quaternion.identity);

        isTeleporting = true;
        StartCoroutine(HandleTeleport());
    }

    private IEnumerator HandleTeleport()
    {
        yield return new WaitForSeconds(1.2f);

        if (runner != null)
        {
            // Stocke le nom de la prochaine room
            RoomTransferManager.NextRoomName = destinationRoomName;

            // Garde le RoomLoader actif entre les scènes
            GameObject loader = new GameObject("RoomTransferLoader");

            loader.AddComponent<RoomTransferManager>();
            DontDestroyOnLoad(loader);
            PortalTransferData.EntryPortalName = this.gameObject.name;

            // Shutdown runner actuel
            yield return runner.Shutdown(true, ShutdownReason.Ok);

            // Recharge la même scène (ou une scène intermédiaire)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
