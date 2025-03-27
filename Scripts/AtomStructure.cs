using UnityEngine;

public class AtomStructure : MonoBehaviour
{
    public GameObject protonPrefab;
    public GameObject neutronPrefab;
    public GameObject electronPrefab;

    public int protonCount = 1;
    public int neutronCount = 0;
    public int electronCount = 1;

    public Transform nucleus;
    public Transform electronShell;
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 100f;

    private GameObject[] electrons;

    void Start()
    {
        // Noyau
        for (int i = 0; i < protonCount; i++)
            Instantiate(protonPrefab, nucleus.position + Random.insideUnitSphere * 0.2f, Quaternion.identity, nucleus);

        for (int i = 0; i < neutronCount; i++)
            Instantiate(neutronPrefab, nucleus.position + Random.insideUnitSphere * 0.2f, Quaternion.identity, nucleus);

        // Électrons en orbite
        electrons = new GameObject[electronCount];
        for (int i = 0; i < electronCount; i++)
        {
            float angle = i * Mathf.PI * 2 / electronCount;
            Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;
            electrons[i] = Instantiate(electronPrefab, electronShell.position + pos, Quaternion.identity, electronShell);
        }
    }

    void Update()
    {
        // Mouvement orbital des électrons
        for (int i = 0; i < electrons.Length; i++)
        {
            if (electrons[i] != null)
                electrons[i].transform.RotateAround(electronShell.position, Vector3.up, orbitSpeed * Time.deltaTime);
        }
    }
}
