using UnityEngine;

public class Bird : MonoBehaviour
{
    public float flySpeed = 10f;
    public float verticalSpeed = 5f;
    public ParticleSystem atomicTrail; // Particules atomiques

    void Start()
    {
        if (atomicTrail != null)
        {
            atomicTrail.Play();
        }
    }

    void Update()
    {
        // Mouvement vers l'avant
        transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);

        // Mouvement vertical basé sur l'input
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.up * verticalInput * verticalSpeed * Time.deltaTime);
    }

    public void BecomeInvincible()
    {
        // Logique pour rendre l'oiseau invincible
    }
}
