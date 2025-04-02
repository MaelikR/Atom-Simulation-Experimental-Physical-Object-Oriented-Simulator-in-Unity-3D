using UnityEngine;

public class BloodFootstepSpawner : MonoBehaviour
{
    public GameObject footprintPrefab;
    public float stepInterval = 0.7f;
    public float duration = 5f;

    private float timer;
    private float lifetime;
    private bool isActive = false;

    public void Activate()
    {
        isActive = true;
        lifetime = duration;
    }

    void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        lifetime -= Time.deltaTime;

        if (timer >= stepInterval)
        {
            timer = 0f;
            SpawnFootprint();
        }

        if (lifetime <= 0f)
            isActive = false;
    }

    void SpawnFootprint()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            GameObject fp = Instantiate(footprintPrefab, hit.point + Vector3.up * 0.01f, Quaternion.LookRotation(transform.forward));
            fp.transform.Rotate(90f, 0f, 0f);
            fp.transform.localScale *= Random.Range(0.8f, 1.2f);
        }
    }
}
