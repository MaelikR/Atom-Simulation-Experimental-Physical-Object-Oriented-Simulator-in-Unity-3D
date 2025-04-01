using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LanternFishController : MonoBehaviour
{
    public float swimSpeed = 2f;
    public float detectionRadius = 10f;
    public Light bioluminescentLight;
    public Color glowColor = new Color(0.1f, 0.6f, 1f);
    public float lightPulseSpeed = 2f;

    private Rigidbody rb;
    private Transform targetLight;
    private float pulseTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        if (bioluminescentLight != null)
        {
            bioluminescentLight.color = glowColor;
        }
    }

    void Update()
    {
        // Cherche une lumière proche
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        targetLight = null;

        foreach (var col in colliders)
        {
            Light foundLight = col.GetComponent<Light>();
            if (foundLight != null && foundLight.enabled)
            {
                targetLight = foundLight.transform;
                break;
            }
        }

        PulseGlow();

        if (targetLight != null)
        {
            MoveTowardsLight();
        }
        else
        {
            Wander();
        }
    }

    void MoveTowardsLight()
    {
        Vector3 direction = (targetLight.position - transform.position).normalized;
        rb.velocity = direction * swimSpeed;
        transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * 2f);
    }

    void Wander()
    {
        rb.velocity = new Vector3(Mathf.Sin(Time.time), Mathf.Cos(Time.time * 0.5f), Mathf.Sin(Time.time * 0.7f)) * 0.5f;
    }

    void PulseGlow()
    {
        if (bioluminescentLight != null)
        {
            pulseTimer += Time.deltaTime * lightPulseSpeed;
            float intensity = Mathf.PingPong(pulseTimer, 1f);
            bioluminescentLight.intensity = intensity * 1.5f;
        }
    }
}
