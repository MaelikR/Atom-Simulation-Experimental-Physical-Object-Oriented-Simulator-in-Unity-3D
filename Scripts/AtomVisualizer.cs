using UnityEngine;

public class AtomVisualizer : MonoBehaviour
{
    public GameObject atomPrefab;
    public int numberOfVisualAtoms = 1000;
    public float spreadRadius = 0.3f;

    void Start()
    {
        for (int i = 0; i < numberOfVisualAtoms; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spreadRadius;
            Instantiate(atomPrefab, randomPos, Quaternion.identity, transform);
        }

    }
}