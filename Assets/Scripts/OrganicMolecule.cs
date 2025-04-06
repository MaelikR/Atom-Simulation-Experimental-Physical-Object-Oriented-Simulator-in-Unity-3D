using UnityEngine;

public class OrganicMolecule : MonoBehaviour
{
    public string structureName = "AcideAminé";
    public bool isSelfReplicating = false;

    public float replicationInterval = 10f;
    private float timer = 0f;

    void Update()
    {
        if (isSelfReplicating)
        {
            timer += Time.deltaTime;
            if (timer >= replicationInterval)
            {
                Replicate();
                timer = 0f;
            }
        }
    }

    void Replicate()
    {
        Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Instantiate(gameObject, transform.position + offset, Quaternion.identity);
        Debug.Log($"{structureName} s'est répliqué !");
    }
}
