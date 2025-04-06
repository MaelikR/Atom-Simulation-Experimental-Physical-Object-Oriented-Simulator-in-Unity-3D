using UnityEngine;

public class QuantumPortal : MonoBehaviour
{
    [Header("Portal Settings")]
    public Transform destination;
    public float cooldown = 2f;
    private bool canTeleport = true;

    [Header("Quantum Effects")]
    public GameObject quantumEffectPrefab;
    public bool rotatePlayer = true;
    public float quantumSpinAmount = 180f;

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport || destination == null) return;

        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    void TeleportPlayer(GameObject player)
    {
        if (quantumEffectPrefab)
        {
            Instantiate(quantumEffectPrefab, player.transform.position, Quaternion.identity);
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        player.transform.position = destination.position;

        if (rotatePlayer)
        {
            player.transform.Rotate(Vector3.up * quantumSpinAmount);
        }

        if (cc) cc.enabled = true;

        if (quantumEffectPrefab)
        {
            Instantiate(quantumEffectPrefab, player.transform.position, Quaternion.identity);
        }

        canTeleport = false;
        Invoke(nameof(ResetTeleport), cooldown);
    }

    void ResetTeleport()
    {
        canTeleport = true;
    }
}
