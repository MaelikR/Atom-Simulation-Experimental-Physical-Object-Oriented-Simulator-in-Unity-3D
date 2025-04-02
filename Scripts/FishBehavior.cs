using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FishGenetics))]
public class FishBehavior : MonoBehaviour
{
    [Header("DNA-Controlled Settings")]
    public float swimSpeed;
    public float turnSpeed;
    public float curiosityRadius;
    public float fleeRadius;
    public float energyDrainRate;
    private FishGenetics genetics;

    [Header("Water Limits")]
    public float waterSurfaceY = 0f;
    public float maxDepth = 5f;

    [Header("Group Behavior")]
    public float neighborRadius = 2f;
    public float separationRadius = 1f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float separationWeight = 1.5f;

    [Header("Life Settings")]
    public float maxLifeTime = 30f;
    public float floatUpSpeed = 0.5f;
    public float deadRotationSpeed = 0.2f;
    public bool IsDead() => isDead;

    [Header("Stimuli & Environment")]
    public Transform player;
    public Transform lightSource;
    public List<string> interestingAtomsTags;
    public List<string> toxicAtoms;
    public List<string> edibleAtoms;
    public float lightAttractionRadius = 4f;

    [Header("Environmental Physics")]
    public AnimationCurve temperatureByDepth;
    public float minComfortTemp = 0.3f;
    public float maxComfortTemp = 0.7f;
    public float fleeFromColdMultiplier = 1.5f;
    public float fleeFromHotMultiplier = 1.5f;

    [Header("Stimuli Weights")]
    public float groupWeight = 1.0f;
    public float stimuliWeight = 1.0f;
    public float baseDirectionWeight = 0.8f;

    [Header("Biological Settings")]
    public float energy = 1f;
    public float lowEnergyThreshold = 0.3f;

    [Header("Water Current")]
    public Vector3 currentDirection = new Vector3(0.2f, 0f, 0.1f);
    public float currentStrength = 0.2f;

    private float age = 0f;
    private bool isDead = false;

    private Rigidbody rb;
    private Vector3 swimDirection;
    private Vector3 lastDirection;
    private float timeSinceLastTurn;
    private Animator animator;
    private Renderer renderer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.drag = 1f;

        animator = GetComponent<Animator>();
        renderer = GetComponentInChildren<Renderer>();
      
        genetics = GetComponent<FishGenetics>();
        if (genetics != null && genetics.dna != null)
        {
            swimSpeed = genetics.dna.swimSpeed;
            turnSpeed = genetics.dna.turnSpeed;
            curiosityRadius = genetics.dna.curiosity;
            fleeRadius = genetics.dna.fleeDistance;
            energyDrainRate = genetics.dna.energyEfficiency;
            if (renderer != null)
                renderer.material.color = genetics.dna.bodyColor;
        }
        ApplyDNA();
        ChooseNewDirection();
        timeSinceLastTurn = Random.Range(0f, 3f);
    }

    void ApplyDNA()
    {
        if (genetics != null && genetics.dna != null)
        {
            swimSpeed = genetics.dna.swimSpeed;
            turnSpeed = genetics.dna.turnSpeed;
            curiosityRadius = genetics.dna.curiosity;
            fleeRadius = genetics.dna.fleeDistance;
            energyDrainRate = genetics.dna.energyEfficiency;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            Vector3 floatUp = new Vector3(0f, floatUpSpeed, 0f);
            Vector3 nextPos = rb.position + floatUp * Time.fixedDeltaTime;
            nextPos.y = Mathf.Min(nextPos.y, waterSurfaceY);
            rb.MovePosition(nextPos);
            Quaternion tilt = Quaternion.Euler(0f, rb.rotation.eulerAngles.y, 90f);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, tilt, deadRotationSpeed * Time.fixedDeltaTime));
            return;
        }

        Vector3 stimuliDir = ComputeStimuliResponse();
        Vector3 groupDir = ComputeGroupBehavior();
        Vector3 blendedDirection = swimDirection * baseDirectionWeight + groupDir * groupWeight + stimuliDir * stimuliWeight;

        swimDirection = Vector3.Slerp(swimDirection, blendedDirection.normalized, 0.1f);
        swimDirection.Normalize();

        Vector3 newPosition = rb.position + swimDirection * swimSpeed * Time.fixedDeltaTime;
        float minY = waterSurfaceY - maxDepth;
        float maxY = waterSurfaceY;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        float idealDepthY = (waterSurfaceY - maxDepth * 0.5f);
        float verticalCorrection = (idealDepthY - rb.position.y) * 0.1f;
        newPosition.y += verticalCorrection;

        rb.MovePosition(newPosition);

        Vector3 flatDirection = new Vector3(-swimDirection.x, 0f, -swimDirection.z);
        if (flatDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }
    }

    void Update()
    {
        if (isDead) return;

        timeSinceLastTurn += Time.deltaTime;
        if (timeSinceLastTurn >= 3f)
        {
            ChooseNewDirection();
            timeSinceLastTurn = 0f;
        }

        energy -= energyDrainRate * Time.deltaTime;
        age += Time.deltaTime;

        if (energy <= lowEnergyThreshold)
            swimSpeed = Mathf.Max(0.5f, swimSpeed * 0.95f);

        if (age >= maxLifeTime)
            Die();

        UpdateDepthColor(); // mise à jour de la couleur camouflage

    }


    public void Die()
    {
        isDead = true;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.drag = 0.5f;
        swimDirection = Vector3.zero;
        if (animator != null)
        {
            animator.enabled = false;
        }


        if (renderer != null)
            renderer.material.color = Color.gray;

        Collider[] nearby = Physics.OverlapSphere(transform.position, 4f);
        foreach (var n in nearby)
        {
            var fish = n.GetComponent<FishBehavior>();
            if (fish != null && !fish.IsDead())
            {
                Vector3 panicDir = (fish.transform.position - transform.position).normalized;
                fish.AddPanicForce(panicDir * 1.5f);

            }
        }
    }

    void ChooseNewDirection()
    {
        Vector3 randomDir = Random.onUnitSphere;
        randomDir.y = Mathf.Clamp(randomDir.y, -0.3f, 0.3f);
        swimDirection = Vector3.Slerp(lastDirection, randomDir, 0.6f);
        lastDirection = swimDirection;
    }

    Vector3 ComputeGroupBehavior()
    {
        Collider[] neighbors = Physics.OverlapSphere(transform.position, neighborRadius);
        Vector3 cohesion = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == this.gameObject || neighbor.GetComponent<FishBehavior>() == null)
                continue;

            Vector3 toNeighbor = neighbor.transform.position - transform.position;
            float distance = toNeighbor.magnitude;

            cohesion += neighbor.transform.position;
            alignment += neighbor.GetComponent<Rigidbody>().velocity;
            if (distance < separationRadius)
                separation -= toNeighbor / distance;

            count++;
        }

        if (count == 0) return Vector3.zero;

        cohesion = ((cohesion / count) - transform.position).normalized * cohesionWeight;
        alignment = (alignment / count).normalized * alignmentWeight;
        separation = separation.normalized * separationWeight;

        return cohesion + alignment + separation;
    }
    // Ajoute ceci à l’intérieur de ta classe FishBehavior (si pas déjà fait)
    public void AddPanicForce(Vector3 panicDirection)
    {
        swimDirection += panicDirection;
    }
    void UpdateDepthColor()
    {
        if (renderer == null) return;

        float depthRatio = Mathf.Clamp01((waterSurfaceY - transform.position.y) / maxDepth);

        // Camouflage de profondeur : surface claire profondeur sombre
        Color shallowColor = new Color(1f, 0.5f, 0.5f); // rose clair en surface
        Color deepColor = new Color(0f, 0.2f, 0.4f);    // bleu foncé en profondeur

        Color adaptedColor = Color.Lerp(shallowColor, deepColor, depthRatio);
        renderer.material.color = adaptedColor;
    }

    Vector3 ComputeStimuliResponse()
    {
        Vector3 response = Vector3.zero;

        if (player != null && Vector3.Distance(transform.position, player.position) < fleeRadius)
            response += (transform.position - player.position).normalized * 2f;

        if (lightSource != null && Vector3.Distance(transform.position, lightSource.position) < lightAttractionRadius)
            response += (lightSource.position - transform.position).normalized * 1.5f;

        foreach (string tag in interestingAtomsTags)
        {
            GameObject[] atoms = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject atom in atoms)
            {
                if (Vector3.Distance(transform.position, atom.transform.position) < curiosityRadius)
                    response += (atom.transform.position - transform.position).normalized;
            }
        }
        


        foreach (string tag in toxicAtoms)
        {
            GameObject[] hazards = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject hazard in hazards)
            {
                if (Vector3.Distance(transform.position, hazard.transform.position) < curiosityRadius)
                    response += (transform.position - hazard.transform.position).normalized * 2f;
            }
        }

        foreach (string tag in edibleAtoms)
        {
            GameObject[] food = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject item in food)
            {
                if (Vector3.Distance(transform.position, item.transform.position) < curiosityRadius)
                    response += (item.transform.position - transform.position).normalized;
            }
        }

        float depthRatio = Mathf.Clamp01((waterSurfaceY - transform.position.y) / maxDepth);
        float tempAtDepth = temperatureByDepth.Evaluate(depthRatio);

        if (renderer != null)
        {
            float ratio = Mathf.InverseLerp(minComfortTemp, maxComfortTemp, tempAtDepth);
            renderer.material.color = Color.Lerp(Color.blue, Color.red, ratio);
        }

        if (tempAtDepth < minComfortTemp)
            response += new Vector3(0f, 0.2f, 0f) * fleeFromColdMultiplier;
        else if (tempAtDepth > maxComfortTemp)
            response += new Vector3(0f, -0.2f, 0f) * fleeFromHotMultiplier;

        return response;
    }
}
