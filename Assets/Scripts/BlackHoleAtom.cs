// =========================
// BlackHoleAtom.cs
// =========================
using UnityEngine;

public class BlackHoleAtom : MonoBehaviour
{
    public float gravitationalRadius = 2f;
    public float pullForce = 10f;
    public ParticleSystem collapseEffect;
    public float collapseTime = 3f;

    private float collapseTimer;

    void Start()
    {
        collapseTimer = collapseTime;
        if (collapseEffect != null)
            Instantiate(collapseEffect, transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        collapseTimer -= Time.deltaTime;

        if (collapseTimer <= 0)
        {
            Destroy(gameObject);
        }

        Collider[] affected = Physics.OverlapSphere(transform.position, gravitationalRadius);
        foreach (Collider col in affected)
        {
            if (col.attachedRigidbody != null && col.gameObject != this.gameObject)
            {
                Vector3 dir = (transform.position - col.transform.position).normalized;
                float distance = Vector3.Distance(transform.position, col.transform.position);
                float force = pullForce / Mathf.Max(distance, 0.1f);
                col.attachedRigidbody.AddForce(dir * force);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, gravitationalRadius);
    }
}