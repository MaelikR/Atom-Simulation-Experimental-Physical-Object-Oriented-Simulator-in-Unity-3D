using UnityEngine;

public class CameraFollowAtom : MonoBehaviour
{
    [SerializeField] private Transform target;       // L'atome à suivre
    [SerializeField] private Vector3 offset = new Vector3(0, 3f, -6f); // Décalage position caméra
    [SerializeField] private float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(target); // La caméra regarde l'atome
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
