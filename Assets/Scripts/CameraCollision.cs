using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform followTarget;

    [Header("Settings")]
    public LayerMask collisionMask;
    public float defaultDistance = 4f;
    public float minDistance = 1f;
    public float collisionRadius = 0.3f;
    public float cameraLerpSpeed = 8f;

    private float currentDistance;

    void Start()
    {
        currentDistance = defaultDistance;
    }

    void LateUpdate()
    {
        if (followTarget == null || cameraTransform == null) return;

        Vector3 origin = followTarget.position;
        Vector3 desiredCameraPos = origin - followTarget.forward * defaultDistance;

        // Check collision
        if (Physics.SphereCast(origin, collisionRadius, -followTarget.forward, out RaycastHit hit, defaultDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, defaultDistance);
        }
        else
        {
            currentDistance = defaultDistance;
        }

        // Move camera
        Vector3 targetPos = origin - followTarget.forward * currentDistance;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * cameraLerpSpeed);

        // Always look at target
        cameraTransform.LookAt(origin);
    }
}
