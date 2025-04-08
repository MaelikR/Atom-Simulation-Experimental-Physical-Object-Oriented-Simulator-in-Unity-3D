using UnityEngine;
using System.Collections.Generic;

public class FishSwarmManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numberOfFish = 30;
    public float spawnRadius = 10f;
    public Transform player;
    [HideInInspector] public List<GameObject> fishes = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            pos.y = Mathf.Clamp(pos.y, transform.position.y - 5f, transform.position.y + 5f); // limite verticale

            GameObject fish = Instantiate(fishPrefab, pos, Quaternion.identity, transform);
            fish.GetComponent<FishSwarmBehavior>().player = player;
            fishes.Add(fish);
        }
    }
}
