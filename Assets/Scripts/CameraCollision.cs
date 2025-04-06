using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("Target à suivre")]
    public Transform target;

    [Header("Réglages collision")]
    public float minDistance = 0.5f;
    public float maxDistance = 4f;
    public float smoothSpeed = 10f;
    public float sphereRadius = 0.3f;
    public LayerMask collisionLayers;

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 direction = (transform.position - target.position).normalized;
        Vector3 origin = target.position + direction * sphereRadius;
        float targetDistance = maxDistance;

        // SphereCast pour éviter les traversées de terrain/objets
        if (Physics.SphereCast(origin, sphereRadius, direction, out RaycastHit hit, maxDistance, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            targetDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }

        Vector3 desiredPosition = target.position + direction * targetDistance;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, Time.deltaTime * smoothSpeed);
        transform.LookAt(target);
    }
}
