// MODULE 1: Manipulation atomique

// AtomMagnet.cs
using UnityEngine;
using Fusion;

public class AtomMagnet : NetworkBehaviour
{
    public float magneticForce = 10f;
    public float radius = 5f;

    void FixedUpdate()
    {
        var atoms = GameObject.FindGameObjectsWithTag("Atom");
        foreach (var atom in atoms)
        {
            float dist = Vector3.Distance(transform.position, atom.transform.position);
            if (dist < radius)
            {
                var rb = atom.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (transform.position - atom.transform.position).normalized;
                    rb.AddForce(dir * magneticForce);
                }
            }
        }
    }
}