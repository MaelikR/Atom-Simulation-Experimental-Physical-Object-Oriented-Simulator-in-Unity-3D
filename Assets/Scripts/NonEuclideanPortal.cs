// NonEuclideanPortal.cs
using UnityEngine;

public class NonEuclideanPortal : MonoBehaviour
{
    public Transform linkedPortal;        // Portail de sortie
    public Vector3 positionOffset = Vector3.zero; // Offset de sortie
    public bool rotateOnTeleport = true;  // Appliquer la rotation du portail de sortie

    private void OnTriggerEnter(Collider other)
    {
        if (linkedPortal == null || !other.CompareTag("Player")) return;

        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            // Désactiver temporairement le controller pour éviter les conflits
            controller.enabled = false;

            // Téléportation avec offset
            other.transform.position = linkedPortal.position + positionOffset;

            // Rotation non-euclidienne
            if (rotateOnTeleport)
                other.transform.rotation = linkedPortal.rotation;

            controller.enabled = true;
        }
    }
}