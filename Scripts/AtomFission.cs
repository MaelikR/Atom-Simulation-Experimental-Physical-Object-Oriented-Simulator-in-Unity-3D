using UnityEngine;

public class AtomFission : MonoBehaviour
{
    public GameObject fragmentPrefab;
    public int fragmentCount = 2;
    public float explosionForce = 5f;
    public ParticleSystem explosionEffect;

    public void TriggerFission()
    {
        // Effet visuel
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Création de fragments
        for (int i = 0; i < fragmentCount; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            GameObject fragment = Instantiate(fragmentPrefab, transform.position + dir * 0.1f, Quaternion.identity);
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(dir * explosionForce, ForceMode.Impulse);
        }

        // Destruction de l'atome original
        Destroy(gameObject);
    }
}
