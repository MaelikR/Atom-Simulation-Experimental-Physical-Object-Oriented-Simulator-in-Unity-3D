// ElectromagneticFieldDistortion.cs
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ElectromagneticFieldDistortion : MonoBehaviour
{
    [Header("Wave Settings")]
    public int segments = 64;
    public float radius = 3f;
    public float pulseSpeed = 2f;
    public float pulseAmplitude = 0.2f;
    public float distortionStrength = 0.1f;

    [Header("Atomic Reaction")]
    public ParticleSystem reactionEffect;
    public LayerMask atomLayer;
    public float reactionRadius = 5f;
    public float reactionCooldown = 3f;

    private LineRenderer line;
    private float angleOffset;
    private float lastReactionTime;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        lastReactionTime = -999f;
    }

    void Update()
    {
        DrawField();
        TriggerReactions();
    }

    void DrawField()
    {
        float angle = 0f;
        angleOffset += Time.deltaTime * pulseSpeed;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = Mathf.Sin(Mathf.Deg2Rad * angle);

            float offset = Mathf.Sin(i * 0.5f + angleOffset) * pulseAmplitude;
            Vector3 pos = transform.position + new Vector3(x, offset * distortionStrength, z) * radius;
            line.SetPosition(i, pos);

            angle += 360f / segments;
        }
    }

    void TriggerReactions()
    {
        if (Time.time - lastReactionTime < reactionCooldown) return;

        Collider[] atoms = Physics.OverlapSphere(transform.position, reactionRadius, atomLayer);
        foreach (var atom in atoms)
        {
            var rb = atom.attachedRigidbody;
            if (rb != null)
            {
                rb.AddExplosionForce(10f, transform.position, reactionRadius);
            }
        }

        if (reactionEffect)
        {
            reactionEffect.transform.position = transform.position;
            reactionEffect.Play();
        }

        lastReactionTime = Time.time;
    }
}
