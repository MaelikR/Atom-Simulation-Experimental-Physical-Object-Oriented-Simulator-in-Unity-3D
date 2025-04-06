using UnityEngine;

public class AtomVisualizer : MonoBehaviour
{
    public GameObject atomPrefab;
    public GameObject organismPrefab;

    public int numberOfVisualAtoms = 1000;
    public float spreadRadius = 0.3f;

    [Range(0f, 1f)]
    public float organismSpawnChance = 0.55f; // 1% de chance qu'un organisme naisse

    void Start()
    {
        for (int i = 0; i < numberOfVisualAtoms; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * spreadRadius;

            GameObject spawned;

            if (Random.value < organismSpawnChance && organismPrefab != null)
            {
                spawned = Instantiate(organismPrefab, randomPos, Quaternion.identity, transform);
             
            }
            else
            {
                spawned = Instantiate(atomPrefab, randomPos, Quaternion.identity, transform);
            }
        }
    }
}
