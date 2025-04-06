using UnityEngine;

public class AtomSpawnerPC : MonoBehaviour
{
    [Header("Atom Prefabs")]
    public GameObject phosphorePrefab;
    public GameObject carbonePrefab;

    [Header("Spawn Settings")]
    public int numberToSpawn = 10;
    public Vector3 spawnAreaSize = new Vector3(10, 5, 10);
    public float minHeight = -4f;
    public float maxHeight = 0f;

    [Header("Randomize Settings")]
    public bool randomizeRotation = true;
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);

    void Start()
    {
        SpawnAtoms(phosphorePrefab);
        SpawnAtoms(carbonePrefab);
    }

    void SpawnAtoms(GameObject prefab)
    {
        if (prefab == null) return;

        for (int i = 0; i < numberToSpawn; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(minHeight, maxHeight),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );

            Quaternion spawnRot = randomizeRotation ? Random.rotation : Quaternion.identity;
            GameObject atom = Instantiate(prefab, spawnPos, spawnRot);

            float scale = Random.Range(scaleRange.x, scaleRange.y);
            atom.transform.localScale *= scale;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * ((maxHeight + minHeight) / 2f), new Vector3(spawnAreaSize.x, maxHeight - minHeight, spawnAreaSize.z));
    }
}