// =========================
// AtomMagneticField.cs — Attraction/Répulsion Magnétique entre Atomes
// =========================
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AtomMagneticField : MonoBehaviour
{
    [Header("Magnetic Settings")]
    public bool isAttractive = true;
    public float magneticStrength = 5f;
    public float magneticRange = 3f;
    public LayerMask atomLayer;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Collider[] nearbyAtoms = Physics.OverlapSphere(transform.position, magneticRange, atomLayer);

        foreach (Collider col in nearbyAtoms)
        {
            if (col.gameObject == gameObject) continue; // Ignore self

            Rigidbody otherRb = col.attachedRigidbody;
            if (otherRb != null)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, col.transform.position);
                float forceMagnitude = magneticStrength / Mathf.Max(distance, 0.1f);

                Vector3 force = direction * forceMagnitude;
                if (!isAttractive) force *= -1;

                rb.AddForce(force);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isAttractive ? Color.blue : Color.red;
        Gizmos.DrawWireSphere(transform.position, magneticRange);
    }
}