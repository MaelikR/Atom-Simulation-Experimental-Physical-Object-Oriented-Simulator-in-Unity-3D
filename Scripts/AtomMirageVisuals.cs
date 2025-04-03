using UnityEngine;

public static class AtomMirageVisuals
{
    public static void SpawnDistortion(Vector3 position)
    {
        // Remplace par ton prefab ou effet visuel réel
        GameObject distortion = Resources.Load<GameObject>("VFX/MirageDistortion");
        if (distortion != null)
        {
            GameObject instance = GameObject.Instantiate(distortion, position, Quaternion.identity);
            GameObject.Destroy(instance, 5f);
        }
    }
}
