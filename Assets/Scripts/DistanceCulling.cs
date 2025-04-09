using UnityEngine;
using Fusion;

public class DistanceCulling : NetworkBehaviour
{
    [Tooltip("Distance à partir de laquelle l'objet sera désactivé.")]
    public float cullingDistance = 150f;

    [Tooltip("Objet à activer/désactiver en fonction de la distance.")]
    public GameObject targetObject;

    private Transform localPlayerTransform;

    private void Start()
    {
        // On cherche le joueur local (autorité)
        foreach (var runnerObj in FindObjectsOfType<NetworkObject>())
        {
            if (runnerObj.HasInputAuthority)
            {
                localPlayerTransform = runnerObj.transform;
                break;
            }
        }

        if (localPlayerTransform == null)
            Debug.LogWarning("DistanceCulling: Aucun joueur local trouvé.");
    }

    private void Update()
    {
        if (localPlayerTransform == null || targetObject == null)
            return;

        float distance = Vector3.Distance(transform.position, localPlayerTransform.position);
        targetObject.SetActive(distance <= cullingDistance);
    }
}
