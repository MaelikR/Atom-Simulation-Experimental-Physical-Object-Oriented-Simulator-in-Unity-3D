using UnityEngine;
using Fusion;

public class AtomicExplosionTrigger : NetworkBehaviour
{
    public float explosionRadius = 10f;
    public float energyAmount = 200f;
    public GameObject explosionVFX;
    public AudioClip explosionSound;

    public void Detonate()
    {
        // VFX + son
        if (explosionVFX) Instantiate(explosionVFX, transform.position, Quaternion.identity);
        if (explosionSound) AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        // Affecte tous les atomes autour
        Collider[] hitAtoms = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var col in hitAtoms)
        {
            Atom atom = col.GetComponent<Atom>();
            if (atom != null)
            {
                atom.AddEnergy(energyAmount); // surcharge
                Rigidbody rb = atom.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 dir = (atom.transform.position - transform.position).normalized;
                    rb.AddExplosionForce(1000f, transform.position, explosionRadius);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) // touche B = boom
        {
            Detonate();
        }
    }
}
