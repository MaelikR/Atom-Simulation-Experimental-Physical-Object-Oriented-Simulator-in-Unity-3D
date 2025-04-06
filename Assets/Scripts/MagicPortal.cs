using UnityEngine;

public class MagicPortal : MonoBehaviour
{
    public Transform targetLocation;
    public ParticleSystem activationEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (activationEffect) activationEffect.Play();
            other.transform.position = targetLocation.position;
        }
    }
}
